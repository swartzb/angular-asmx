'use strict';

/* Filters */

angular.module('myApp.filters', ['myApp.services']).
  filter('interpolate', ['version', function(version) {
    return function(text) {
      return String(text).replace(/\%VERSION\%/mg, version);
    };
  }]);
