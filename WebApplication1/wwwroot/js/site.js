// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function() {
  var formToSubmit = null;

  // Silme butonuna tıklanınca modalı aç
  $('.btn-delete').on('click', function(e) {
    e.preventDefault();
    formToSubmit = $(this).closest('form');
    $('#confirmModal').modal('show');
  });

  // Modal’daki “Evet, Sil” butonuna tıklanınca formu gönder
  $('#confirmDeleteBtn').on('click', function() {
    if (formToSubmit) {
      formToSubmit.submit();
      formToSubmit = null;
      $('#confirmModal').modal('hide');
    }
  });
});                   