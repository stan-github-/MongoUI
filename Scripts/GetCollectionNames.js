var GetCollectionNames = function () {
    var x = db.getCollectionNames();
    x.forEach(function (x) { print(x); });
}