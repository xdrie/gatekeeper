// https://stackoverflow.com/questions/41431322/how-to-convert-formdatahtml5-object-to-json
function parseFormData(formData) {
    var object = {};
    formData.forEach((value, key) => {
        // Reflect.has in favor of: object.hasOwnProperty(key)
        if (!Reflect.has(object, key)) {
            object[key] = value;
            return;
        }
        if (!Array.isArray(object[key])) {
            object[key] = [object[key]];
        }
        object[key].push(value);
    });
    return object;
}

$("#auth").addEventListener("submit", ev => {
    let formData = new FormData(ev.target);
    let authData = parseFormData(formData);

    console.log('submitting auth', authData);

    ev.preventDefault();
});