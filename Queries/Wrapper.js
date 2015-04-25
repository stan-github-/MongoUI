(function () {

    /*phantom.onError = function (msg, trace) {
        var msgStack = ['PHANTOM ERROR: ' + msg];
        if (trace && trace.length) {
            msgStack.push('TRACE:');
            trace.forEach(function (t) {
                msgStack.push(' -> ' + (t.file || t.sourceURL) + ': ' + t.line + (t.function ? ' (in function ' + t.function + ')' : ''));
            });
        }
        console.error(msgStack.join('\n'));
        console.log(msgStack.join('\n'));
        phantom.exit(1);
    };*/

    var page = require('webpage').create();
    var system = require('system');

    page.onConsoleMessage = function (msg) {
        system.stdout.writeLine(msg);
        //system.stderr.writeLine( 'window console: ' + msg);
    };

    /*page.onError = function (msg, trace) {

        var msgStack = ['ERROR: ' + msg];

        if (trace && trace.length) {
            msgStack.push('TRACE:');
            trace.forEach(function (t) {
                msgStack.push(' -> ' + t.file + ': ' + t.line + (t.function ? ' (in function "' + t.function + '")' : ''));
            });
        }

        console.error(msgStack.join('\n'));
        console.log(msgStack.join('\n'));
    };*/

    
    var htmlFilePath = '{replace_replace_replace}';
    page.open(htmlFilePath,
      function (status) {
          console.log('phantom: page open status: ' + status);
          phantom.exit(0);
      }
    );

})();