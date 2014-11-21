

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

//var ds = "/Date(-664736400000)/";
//var re2 = /^\/Date\((-?\d+)\)\/$/;
////var re2 = /^\/Date\(-?d+\)\/$/;
//var res2 = re2.exec(ds);
//console.log(res2);
//console.log(res2[1]);
//console.log(parseInt(res2[1]));
//console.log(new Date(parseInt(res2[1])));
//console.log(new Date(parseInt(res2[1])).toLocaleDateString("en-US"));
console.log(JSON.stringify(new Date()));

(function () {
  /* code */
  var ds = "/Date(-664736400000)/";
  var re = /-?\d+/;
//  var re = /^\/Date\((-?\d+)\)\/$/;
  var res = re.exec(ds);
  console.log(res);
  console.log(res[0]);
  console.log(parseInt(res[0]));
  var d = new Date(parseInt(res[0]));
  console.log(d);
  var s = d.toLocaleDateString("en-US");
  console.log(s);
  console.log(d.toDateString());
}()); // Crockford recommends this one

(function () {
  /* code */
  var ds = "09/11/2001";
  var arr = ds.split('/');
  console.log(arr);
  var d = arr[2] + '-' + arr[0] + '-' + arr[1];
  console.log(d);
}()); // Crockford recommends this one

(function () {
  var re = /^\/Date\((-?\d+)\)\/$/;
  var res1 = re.exec("/Date(-664736400000)/");
  console.log(res1);
  var res2 = re.exec("10/8/1947");
  console.log(res2);
}()); // Crockford recommends this one
