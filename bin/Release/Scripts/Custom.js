var printz = function (x) {
    if (x._collection) {
        x.forEach(function (z) { printjson(z); });
        return;
    }

    printjson(x);
};