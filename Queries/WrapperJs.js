(function () {

    var page = require('webpage').create();
    var system = require('system');

    page.onConsoleMessage = function (msg) {
        system.stdout.writeLine(msg);
        //system.stderr.writeLine( 'window console: ' + msg);
    };

    var htmlFilePath = '{replace_replace_replace}';
    page.open(htmlFilePath,
      function (status) {
          console.log('phantom: page open status: ' + status);
          phantom.exit(0);
      }
    );

})();