(function ($) {
    $.extend({
        validate: function () {
            var hasValidation = false;

            $("input[data-required],textarea[data-required]").each(function () {
                if ($(this).val() == "") {
                    $(this).addClass("is-invalid");
                    $(this).siblings(".invalid-feedback").text($(this).attr("data-required"));
                    hasValidation = true;
                }
                else {
                    $(this).removeClass("is-invalid");
                    $(this).siblings(".invalid-feedback").text("");
                }
            });


            $("input[data-equals-id]").each(function () {

                if ($(this).siblings(".invalid-feedback").text() != "") {
                    return false;
                }

                if ($("#" + $(this).attr("data-equals-id")).val() != $(this).val()) {
                    $(this).addClass("is-invalid");
                    $(this).siblings(".invalid-feedback").text($(this).attr("data-notequals"));

                    hasValidation = true;
                }
                else {
                    $(this).removeClass("is-invalid");
                    $(this).siblings(".invalid-feedback").text("");
                }
            });

            return !hasValidation;
        }
    });
}(jQuery));