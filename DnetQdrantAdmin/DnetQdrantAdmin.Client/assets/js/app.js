
window.qdrantapp = (function () {
    return {
        prettyPrintJsonData: function (elementRef, jsonString) {
            try {
                const data = JSON.parse(jsonString);

                if (elementRef) {
                    elementRef.innerHTML = prettyPrintJson.toHtml(data);
                } else {
                    console.error('Elemento referenciado no encontrado');
                }
            } catch (error) {
                console.error('Error al procesar o mostrar JSON:', error);
            }
        }
    };
})();
