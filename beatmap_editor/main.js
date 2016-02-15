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
				var offline_ctx = new OfflineAudioContext();
				var os = offline_ctx.createBufferSource();
				os.buffer = buf;
				os.connect(offline_ctx.destination);

				offline_ctx.oncomplete = function(e) {
					var source = audio_ctx.createBufferSource();
					source.buffer = buf;

					glob = {
						type: 'editing',
						source: source,
					};
				};

				os.start();
				offline_ctx.startRendering();
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
});

var on_click = function(me) {
	if(glob.type === 'wait_for_audio'  ||  glob.type === 'wait_for_beatmap')
		uploader.click();
};

var update = function(elapsed) {
};

var draw = function(ctx) {
	if(glob.type === 'wait_for_audio') {
		ctx.fillText('Click to upload an audio file.', 50, 50);
	} else if(glob.type === 'wait_for_beatmap') {
		ctx.fillText('Click to upload a beatmap file.', 50, 50);
		ctx.fillText('Press spacebar to skip this step and just start '
		             + with an empty beatmap.', 50, 100);
	} else if(glob.type === 'editing') {
		
	}
};
