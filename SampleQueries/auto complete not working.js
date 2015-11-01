var x = {z: 1, y: 2};

x.y = 1;
x.z = 2;

db.test.insert({x:1, y:2});

var z = db.test.find().count();
printjson(z);

function d(){
   var z = db.test.getCollection();
   var g = z.find();
   printjson('g');
   printjson(g);
}

d();
printjson(z);

/*var a = function z(p){
	printjson(p - 1);
}

var b = function y(q){
	printjson(q + 1 );
}*/

//var z = db.test.find({x:1}, {z:1}).count();

