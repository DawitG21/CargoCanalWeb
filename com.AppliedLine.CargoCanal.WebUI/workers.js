var api = 'http://localhost:49931/api';
var count = 0;
var token = null;

function sessionTimeoutWorker() {
    count++;

    // pass data to the worker
    onmessage = function (e) {
        console.log('data received', e.data);
        token = e.data;
        //if (e.data !== '') token = e.data;
    };

    // get the latest token & pass it to the main caller
    postMessage(token);

    if (count === 6) postMessage('a0bc9cfc-ad22-487e-9f3d-f692cd120f06');

    setTimeout('sessionTimeoutWorker()', 5000);
}

sessionTimeoutWorker();