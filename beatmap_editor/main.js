/*
Citation:
http://stackoverflow.com/questions/3665115/create-a-file-in-memory-for-user-to-download-not-through-server
2016 Feb 15, 5:04pm PST
*/
var download = function(filename, text) {
	var element = document.createElement('a');
	element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(text));
	element.setAttribute('download', filename);

	element.style.display = 'none';
	document.body.appendChild(element);

	element.click();

	document.body.removeChild(element);
};

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
	var file = this.files[0];
	var filename = file.name;

	if(glob.type === 'wait_for_audio') {
		fr.onload = function(e) {
			audio_ctx.decodeAudioData(e.target.result, function(buf) {
				glob = {
					type: 'wait_for_beatmap',
					buffer: buf,
					playback: {
						type: 'stopped',
						position: 0,
					},
					s0: 0,
					s1: buf.length,
					filename: filename,
				};
			});
		};
		fr.readAsArrayBuffer(file);
	} else if(glob.type === 'wait_for_beatmap') {
		fr.onload = function(e) {
			var json = JSON.parse(e.target.result);

			glob.type = 'editing';
			glob.beatmap = json;
			glob.dragging = {type: false};
			glob.filename = filename;
		};
		fr.readAsText(this.files[0]);
	}
};

var on_click = function(me) {
	if(glob.type === 'wait_for_audio'  ||  glob.type === 'wait_for_beatmap')
		uploader.click();
};

var keydown = function(ke) {
	if(ke.keyCode === 32) {
		if(glob.type === 'wait_for_beatmap') {
			glob.type = 'editing';
			glob.dragging = {type: false};
			glob.beatmap = [];
		} else if(glob.type === 'editing') {
			if(glob.playback.type === 'stopped') {
				var source = audio_ctx.createBufferSource();
				source.buffer = glob.buffer;
				source.connect(audio_ctx.destination);
				var time = audio_ctx.currentTime;
				source.start(time + 0.15, glob.playback.position);

				var start_time = time + 0.15 - glob.playback.position;
				glob.playback = {
					type: 'playing',
					source: source,
					start_time: start_time,
				};
			} else if(glob.playback.type === 'playing') {
				glob.playback.source.stop();

				var position = audio_ctx.currentTime - glob.playback.start_time;
				glob.playback = {
					type: 'stopped',
					position: position,
				};
			}
		}
	} else if(ke.keyCode === 27) {
		if(glob.type === 'editing') {
			var filename = glob.filename;

			// Set the file type to .beatmap
			var a = filename.split('.');
			a.pop();
			if(a.length === 0)
				a.push('file');
			a.push('beatmap');
			filename = a.join('.');

			download(filename, JSON.stringify(glob.beatmap));
		}
	}
};

var update = function(elapsed) {
};

var draw_waveform = function(ctx, w, h) {
	// Draw waveform.
	var buf = glob.buffer;
	var array = buf.getChannelData(0);
	var y = function(s) {return Math.floor(h * (1-s) / 2);};
	var start = glob.s0;
	var finish = glob.s1;
	var step = w / (finish-start);
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
		ctx.moveTo(0, h/2);
		for(var i=start; i<finish; ++i) {
			var x = (i-start) * step;
			if(i < 0 || i >= buf.length)
				ctx.moveTo(x, y(0));
			else
				ctx.lineTo(x, y(array[i]));
		}
	}
	ctx.stroke();
};

// Converts a song "time" coordinate into a screen "pixel" coordinate.
// Takes the current "camera" into account.
var t2p = function(pos) {
	assert(glob.type === 'editing');

	var s0 = glob.s0;
	var s1 = glob.s1;

	var temp = pos;
	temp *= glob.buffer.length / glob.buffer.duration;  // sampleRate?
	temp -= s0;
	temp /= (s1-s0);
	temp = 1 - temp;
	temp *= HEIGHT;
	return temp;
};
var p2t = function(y) {
	assert(glob.type === 'editing');

	var temp = y;
	temp /= HEIGHT;
	temp = 1 - temp;
	temp *= glob.s1 - glob.s0;
	temp += glob.s0;
	temp *= glob.buffer.duration / glob.buffer.length;  // sampleRate?
	return temp;
};
// Returns the current zoom level in "pixels per second of music"
var scale = function() {
	var temp = glob.s1 - glob.s0;  // Samples per screen
	temp = 1/temp;                 // Screens per sample
	temp *= HEIGHT;                // Pixels per sample
	temp *= glob.buffer.length;    // Pixels
	temp /= glob.buffer.duration;  // Pixels per second
	return temp;
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
		// Draw sideways waveform.
		ctx.save();
		ctx.rotate(-Math.PI / 2);
		ctx.translate(-HEIGHT, WIDTH/2);
		draw_waveform(ctx, HEIGHT, WIDTH/2);
		ctx.restore();

		// Draw the "beats"
		ctx.fillStyle = 'blue';
		var scal = scale();
		for(var i=0; i<glob.beatmap.length; ++i) {
			var beat = glob.beatmap[i];
			ctx.fillRect(0, t2p(beat.time) - scal*.1 , HEIGHT/2/10, scal*.2);
		}

		// Draw current playback position
		if(glob.playback.type === 'playing') {
			var y = t2p(audio_ctx.currentTime - glob.playback.start_time);
			ctx.fillStyle = 'red';
			ctx.fillRect(0, y, WIDTH, 2);
		}
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

	var s0 = glob.s0;
	var s1 = glob.s1;

	ms = s0 + (s1 - s0) * (1 - my/HEIGHT);

	var ns0 = ms + (s0-ms)*k;
	var ns1 = ms + (s1-ms)*k;

	if(ns1 - ns0 >= 10  &&  ns1 - ns0 <= glob.buffer.length*2)
		set_camera(ns0, ns1)
};

var mousedown = function(e) {
	if(glob.type !== 'editing')
		return;

//	const rect = game.canvas.getBoundingClientRect();
	var mx = e.clientX;
	var my = e.clientY;

	if(e.button === 0) {  // left
		var beat = {
			time: p2t(my),
		};
		glob.beatmap.push(beat);
	} else if(e.button === 2) {  // right
		glob.dragging = {
			type: true,
			x: mx,
			s0: glob.s0,
			s1: glob.s1,
			y: my,
		};
	}
};

var mouseup = function(e) {
	if(glob.type !== 'editing')
		return;

	if(e.button !== 2)
		return;

	glob.dragging.type = false;
};

var set_camera = function(ns0, ns1) {
	var ms;
	var ds;

	ms = (ns0 + ns1) / 2;
	ds = ms - glob.buffer.length;
	if(ds > 0) {
		ns0 -= ds;
		ns1 -= ds;
	}

	ms = (ns0 + ns1) / 2;
	ds = 0 - ms;
	if(ds > 0) {
		ns0 += ds;
		ns1 += ds;
	}

	glob.s0 = Math.floor(ns0);
	glob.s1 = Math.floor(ns1);
};

var mousemove = function(e) {
	if(glob.type !== 'editing'  ||  glob.dragging.type !== true)
		return;

//	const rect = game.canvas.getBoundingClientRect();
	var mx = e.clientX;
	var my = e.clientY;

	var dy = my - glob.dragging.y;

	var s0 = glob.dragging.s0;
	var s1 = glob.dragging.s1;
	var scale = (s1 - s0) / HEIGHT;
	var ns0 = s0 + scale * dy;
	var ns1 = s1 + scale * dy;
	set_camera(ns0, ns1);
};
