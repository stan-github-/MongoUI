/*var printz = function (x) {
    if (x._collection) {
        x.forEach(function (z) { printjson(z); });
        return;
    }

    printjson(x);
};*/



var printjson =
    (function () {
        var printOriginal = printjson;

        var f = 
            function (x) {
                if (x._collection) {
                    x.forEach(function (z) { printOriginal(z); });
                    return;
                }
    
                printOriginal(x);
            };

        return f;
    })();
    