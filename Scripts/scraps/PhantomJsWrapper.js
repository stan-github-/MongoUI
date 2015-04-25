(function () {
    var page = require('webpage').create();
    var system = require('system');
    
    var pageUrl = '{replace_replace_replace}';
    page.open(pageUrl,
        function (status) {
            console.log('phantom: ' + status);
            phantom.exit(0);
        });

    page.onLoadStarted = function () {
        //console.log('page load started');
    };

    page.onLoadFinished = function () {
        //loadInProgress = false;
        //console.log('page loaded');
    };

    page.onConsoleMessage =
        function (msg, lineNum, sourceId) {
            console.log(msg);
    };
})();

/*
(function () {

    var page = require('webpage').create()
    var system = require('system');
    var foo = 42;

    page.onConsoleMessage = function (msg) {
        system.stderr.writeLine('console: ' + msg);
    };

    function evaluate(page, func) {
        var args = [].slice.call(arguments, 2);
        var fn = "function() { return (" + func.toString() + ").apply(this, " + JSON.stringify(args) + ");}";
        return page.evaluate(fn);
    }

    page.open(
      'about:blank',
      function () {
          var foo = 67;
          evaluate(
            page,
            function (foo) {
                console.log(foo);
            },
            foo
          );

          console.log("Done");

          phantom.exit(0); // must exit somewhere in the script
      }
    );
})();
*/