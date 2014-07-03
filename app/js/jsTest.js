

var ds = '4/5/15';
var d1 = new Date(ds);
console.log(d1);
var d2 = Date.parse(ds);
console.log(d2);
var d3 = new Date(d2);
console.log(d3);
var d4 = Date.parse('abc');
console.log(d4);
var d5 = new Date('abc');
console.log(d4);

var rightNow = new Date();
console.log(rightNow);
var today = new Date(rightNow.toDateString());
console.log(today);

var re = /-?\d+/;
var result = re.exec("/Date(-713772000000)/");
console.log(result);

var ds = "/Date(750837600000)/";
var re2 = /^\/Date\(-?d+\)\/$/;
var res2 = re2.exec(ds);
console.log(res2);



