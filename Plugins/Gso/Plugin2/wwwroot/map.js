window.plugin2MapExtensions = {

    generateRandomMarker: function () {
        const map = window.gso.map;
        const lat = this.generateNumber(38.552130160424475, 38.663334192175874);
        const lng = this.generateNumber(-9.189644590301661, -9.009056821746974);

        new google.maps.Marker({
            position: { lat: lat, lng: lng },
            map: map,
            icon: { url: '/plugins/resources/car.png', scaledSize: new google.maps.Size(48, 48) },
            title: "Hello World!",
        });
    },

    generateNumber: function (min, max) {
        return min + (max - min) * Math.random();
    }
};