var assert = function(b) {
	if(b) {
		// Do nothing.
	} else {
		debugger;
		throw 'Assertion failed.'
	}
};

var WIDTH = 640;
var HEIGHT = 480;

var uploader;
var audio_ctx;

var glob = {type: 'wait_for_audio'};

window.onload = function() {
	// Confirm page navigation
	window.onbeforeunload = function(e) {
		e = e || window.event;
		var message = 'Are you sure you want to close the editor?';
		if(e)
			e.returnValue = message;
		return message;
	};

	// Disable context menu
	document.body.oncontextmenu = function(e) {return false;};

	// Create canvas.
	var canvas = document.createElement('canvas');
	canvas.style.width = '100%';
	canvas.style.height = '100%';
	canvas.style.position = 'absolute';
	canvas.style.top = '0';
	canvas.style.left = '0';
	canvas.setAttribute('width', WIDTH);
	canvas.setAttribute('height', HEIGHT);
	document.body.appendChild(canvas);

	// Create file-upload form element.
	uploader = document.createElement('input');
	uploader.setAttribute('type', 'file');
	uploader.addEventListener('change', on_file_select);
	uploader.style.display = 'none';
	document.body.appendChild(uploader);

	// Register event listeners.
	canvas.addEventListener('mousemove', mousemove);
	canvas.addEventListener('mousedragged', mousemove);
	canvas.addEventListener('mousedown', mousedown);
	canvas.addEventListener('mouseup', mouseup);
	canvas.addEventListener('click', on_click);
	canvas.addEventListener('wheel', on_wheel);
	document.addEventListener('keydown', keydown);

	// Get the canvas context.
	var ctx = canvas.getContext('2d');

	// Initialize web audio.
	audio_ctx = new AudioContext();

	// Kick off the update loop.
	var on_anim = (function() {
		var time = -1;
		return function(timestamp) {
			if(time < 0)
				time = timestamp - 16;

			update(Math.min(100, timestamp - time));
			ctx.clearRect(0, 0, WIDTH, HEIGHT);
			draw(ctx);

			time = timestamp;
			requestAnimationFrame(on_anim);
		};
	}());
	requestAnimationFrame(on_anim);
};

var on_file_select = function() {
	var fr = new FileReader();

	if(glob.type === 'wait_for_audio') {
		fr.onload = function(e) {
			audio_ctx.decodeAudioData(e.target.result, function(buf) {
				var source = audio_ctx.createBufferSource();
				source.buffer = buf;

				glob = {
					type: 'wait_for_beatmap',
					buffer: buf,
					source: source,
					x_left: 0,
					x_right: buf.length,
				};
			});
		};
		fr.readAsArrayBuffer(this.files[0]);
	} else if(glob.type === 'wait_for_beatmap') {
		fr.onload = function(e) {
			var json = JSON.parse(e.target.result);
			assert(false);
			glob = {type: 'editing'};
		};
		fr.readAsText(this.files[0]);
	}
};

var on_click = function(me) {
	if(glob.type === 'wait_for_audio'  ||  glob.type === 'wait_for_beatmap')
		uploader.click();
};

var keydown = function(ke) {
	if(ke.keyCode !== 32)
		return;

	if(glob.type === 'wait_for_beatmap') {
		glob.type = 'editing';
		glob.dragging = false;
	}
};

var update = function(elapsed) {
};

var draw = function(ctx) {
	ctx.canvas.width = WIDTH = window.innerWidth;
	ctx.canvas.height = HEIGHT = window.innerHeight;

	if(glob.type === 'wait_for_audio') {
		ctx.fillText('Click to upload an audio file.', 50, 50);
	} else if(glob.type === 'wait_for_beatmap') {
		ctx.fillText('Click to upload a beatmap file.', 50, 50);
		ctx.fillText('Press spacebar to skip this step and just start '
		             + 'with an empty beatmap.', 50, 100);
	} else if(glob.type === 'editing') {
		// Draw waveform.
		var buf = glob.buffer;
		var array = buf.getChannelData(0);
		var y = function(s) {return Math.floor(HEIGHT * (1-s) / 2);};
		var start = glob.x_left;
		var finish = glob.x_right;
		var step = WIDTH / (finish-start);
		ctx.beginPath();
		if(step < 0.25) {
			var i;
			var base_x;
			if(start < 0) {
				i = 0;
				base_x = Math.floor(-start * step);
			} else {
				i = start;
				base_x = 0;
			}
			var min = 0;
			var max = 0;
			ctx.beginPath();
			for(; i<finish; ++i) {
				if(i >= buf.length)
					break;

				var x = (i-start) * step;

				while(x >= base_x+1) {
					ctx.moveTo(base_x+.5, y(min)+1);
					ctx.lineTo(base_x+.5, y(max)-1);

					++base_x;
					min = 1;
					max = -1;
				}

				min = Math.min(min, array[i]);
				max = Math.max(max, array[i]);
			}
		} else {
			ctx.moveTo(0, HEIGHT/2);
			for(var i=start; i<finish; ++i) {
				var x = (i-start) * step;
				if(i < 0 || i >= buf.length)
					ctx.moveTo(x, y(0));
				else
					ctx.lineTo(x, y(array[i]));
			}
		}
		ctx.stroke();
	}
};

var on_wheel = function(e) {
	if(glob.type !== 'editing')
		return;

	if(glob.dragging.type === true)
		return;

//	const rect = game.canvas.getBoundingClientRect();
	var mx = e.clientX;
	var my = e.clientY;

	var k = Math.exp(e.deltaY / 20);

	var x0 = glob.x_left;
	var x1 = glob.x_right;

	xm = x0 + (x1-x0)*mx/WIDTH;

	var nx0 = xm + (x0-xm)*k;
	var nx1 = xm + (x1-xm)*k;

	if(nx1 - nx0 >= 10  &&  nx1 - nx0 <= glob.buffer.length*2)
		set_camera(nx0, nx1)
};

var mousedown = function(e) {
	if(glob.type !== 'editing')
		return;

	if(e.button !== 2)
		return;

//	const rect = game.canvas.getBoundingClientRect();
	var mx = e.clientX;
	var my = e.clientY;

	glob.dragging = {
		type: true,
		x: mx,
		x0: glob.x_left,
		x1: glob.x_right,
		y: my,
	};
};

var mouseup = function(e) {
	if(glob.type !== 'editing')
		return;

	if(e.button !== 2)
		return;

	glob.dragging.type = false;
};

var set_camera = function(nx0, nx1) {
	var m;
	var dx;

	m = (nx0 + nx1) / 2;
	dx = m - glob.buffer.length;
	if(dx > 0) {
		nx0 -= dx;
		nx1 -= dx;
	}

	m = (nx0 + nx1) / 2;
	dx = 0 - m;
	if(dx > 0) {
		nx0 += dx;
		nx1 += dx;
	}

	glob.x_left = Math.floor(nx0);
	glob.x_right = Math.floor(nx1);
};

var mousemove = function(e) {
	if(glob.type !== 'editing'  ||  glob.dragging.type !== true)
		return;

//	const rect = game.canvas.getBoundingClientRect();
	var mx = e.clientX;
	var my = e.clientY;

	var dx = glob.dragging.x - mx;

	var x0 = glob.dragging.x0;
	var x1 = glob.dragging.x1;
	var scale = (x1 - x0) / WIDTH;
	var nx0 = x0 + scale * dx;
	var nx1 = x1 + scale * dx;
	set_camera(nx0, nx1);
};
