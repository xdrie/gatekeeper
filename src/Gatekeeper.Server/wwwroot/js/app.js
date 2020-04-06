// app-wide script
const $ = document.querySelector.bind(document)

Element.prototype.show = function() {
    this.style.display = '';
}

Element.prototype.hide = function() {
    this.style.display = 'none';
}

Element.prototype.toggle = function() {
    if (this.style.display == 'none') {
        this.show();
    } else {
        this.hide();
    }
}
