var HEIGHT = 480;
var WIDTH = 640;

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

	// Create canvas.
	var canvas = document.createElement('canvas');
	canvas.setAttribute('width', '640');
	canvas.setAttribute('height', '480');
	document.body.appendChild(canvas);

	// Create file-upload form element.
	uploader = document.createElement('input');
	uploader.setAttribute('type', 'file');
	uploader.addEventListener('change', on_file_select);
	uploader.style.display = 'none';
	document.body.appendChild(uploader);

	// Register event listeners.
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
	}
};

var update = function(elapsed) {
};

var draw = function(ctx) {
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
		var y = function(s) {return HEIGHT * (1-s) / 2;};
		var start = glob.x_left;
		var len = glob.x_right - start;
		var step = WIDTH/len;
		ctx.beginPath();
		if(step < 0.5) {
			var base_x = 0;
			var min = 0;
			var max = 0;
			ctx.beginPath();
			for(var i=start; i<len; ++i) {
				var x = i*step;

				while(x >= base_x+1) {
					ctx.moveTo(base_x+.5, y(min)-1);
					ctx.lineTo(base_x+.5, y(max)+1);

					++base_x;
					min = 1;
					max = -1;
				}

				min = Math.min(min, array[i]);
				max = Math.max(max, array[i]);
			}
		} else {
			ctx.moveTo(0, HEIGHT/2);
			for(var i=start; i<len; ++i) {
				var x = i*step;
				ctx.lineTo(x, y(array[i]));
			}
		}
		ctx.stroke();
	}
};

var on_wheel = function(e) {
	if(glob.type !== 'editing')
		return;

	var k = Math.exp(e.deltaY / 20);
	var n = glob.x_right * k;
	glob.x_right = Math.max(10, Math.min(n, glob.buffer.length));
};
