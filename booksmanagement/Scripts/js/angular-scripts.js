var myApp = angular.module('myApp', [
    'ui.bootstrap',
    'ngSanitize',
    'toaster',
    'cp.ngConfirm'
]);

var root = $('#rootPath').attr('href');