function arrangeAccident($element, probability, highPlayer) {
    if (Math.random() * 100 <= probability) {
        $element.addClass("broken");

        var player = Math.floor((Math.random() * highPlayer + 1));

        $element.text("Spiller nummer " + player + " er skadet!");
    }
    else {
        $element.addClass("fixed");
        $element.text("Uskadd tropp");
    }
}

$(document).ready(function () {
    $('#break').click(function () {
        arrangeAccident($('#home'), $('#probability').val(), $('#high-player').val());
        arrangeAccident($('#away'), $('#probability').val(), $('#high-player').val());
        $('#break').attr('disabled', true);
        $('#reset').attr('disabled', false);
    });

    $('#reset').click(function () {
        $('#home').removeClass("broken").removeClass("fixed").text("-");
        $('#away').removeClass("broken").removeClass("fixed").text("-");
        $('#break').attr('disabled', false);
        $('#reset').attr('disabled', true);
    });

    $('#break').attr('disabled', false);
    $('#reset').attr('disabled', true);
});