var printjson =
    (function () {
        var printOriginal = printjson;

        var f = 
            function (x) {
                if (x._collection) {
                    x.forEach(function (z) { printOriginal(z); print(',')});
                    return;
                }
    
                printOriginal(x);
            };

        return f;
    })();
    