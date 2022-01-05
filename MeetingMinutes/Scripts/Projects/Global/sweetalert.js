var sa = angular.module('sweetalert', []);

sa.factory('swal', SweetAlert);

function SweetAlert() {
    return window.swal;
};