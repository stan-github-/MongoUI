/*var printz = function (x) {
    if (x._collection) {
        x.forEach(function (z) { printjson(z); });
        return;
    }

    printjson(x);
};*/

var printOriginal = printjson;

var printjson = function (x) {
    if (x._collection) {
        x.forEach(function (z) { printOriginal(z); });
        return;
    }
    
    printOriginal(x);
};