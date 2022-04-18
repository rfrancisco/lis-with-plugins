let map;
let markers;
let dotNetObjRef;
let infoWindow;

/*******************************************************************************
 * Initializaes the google map.
 * 
 * @param {any} options The map initialization options.
 * @param {any} objRef The dotnet object reference.
 *******************************************************************************/
export function initMap(options, objRef) {
    dotNetObjRef = objRef;

    let mapOptions = {
        center: { lat: options.lat, lng: options.lng },
        zoom: options.defaultZoom ?? 12,
        minZoom: options.minZoom ?? 8,
        maxZoom: options.maxZoom ?? 22,
        restriction: !options.boundingBox ? null : { latLngBounds: options.boundingBox, strictBounds: false },
        zoomControl: false,
        mapTypeControl: false,
        scaleControl: false,
        streetViewControl: true,
        rotateControl: false,
        fullscreenControl: false,
        mapTypeControlOptions: {
            mapTypeIds: ["roadmap", "hybrid"],
            style: google.maps.MapTypeControlStyle.DEFAULT,
        }
    };

    // Initializes a new google map on the html element
    map = new google.maps.Map(document.getElementById(options.mapId ?? 'map'), mapOptions);

    // Initializes a info-window
    infoWindow = new google.maps.InfoWindow();

    // Initializes the array of markers
    markers = [];

    // Attaches the map object to a global object so it can be accessed by others
    if (!window.gso)
        window.gso = { map: map };
    else
        window.gso.map = map;
}

/*******************************************************************************
 * Set the map type id (ex: roadmap, hybrid, etc...)
 *******************************************************************************/
export function setMapTypeId(mapTypeId) {
    map.setMapTypeId(mapTypeId);
}

/*******************************************************************************
 * Get the current map type id (ex: roadmap, hybrid, etc...)
 *******************************************************************************/
export function getMapTypeId() {
    return map.getMapTypeId();
}

/*******************************************************************************
 * Applies the next available map type id (ex: roadmap, hybrid, etc...)
 *******************************************************************************/
export function setNextAvailableMapTypeId() {
    const mapTypes = map.mapTypeControlOptions.mapTypeIds;
    // Doubles the array to simplify getting the next id of the last index
    const doubleMapTypes = mapTypes.concat(mapTypes);
    // Get the next mapTypeId
    const nextMapTypeId = doubleMapTypes[doubleMapTypes.findIndex(i => i == map.getMapTypeId()) + 1];
    // Apply the next available mapTypeId
    map.setMapTypeId(nextMapTypeId);
}

/*******************************************************************************
 * If supported shows the user location on the map.
 *******************************************************************************/
export function showUserCurrentLocation() {
    // Try HTML5 geolocation.
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
            (position) => {
                map.setCenter({ lat: position.coords.latitude, lng: position.coords.longitude });
                map.setZoom(17);
            },
            () => {
                handleLocationError(true, infoWindow, map.getCenter());
            }
        );
    } else {
        // Browser doesn't support Geolocation
        handleLocationError(false, infoWindow, map.getCenter());
    }
}

/*******************************************************************************
 * Increases the zoom level of the map
 *******************************************************************************/
export function zoomIn() {
    map.setZoom(map.getZoom() + 1);
}

/*******************************************************************************
 * Decreases the zoom level of the map
 *******************************************************************************/
export function zoomOut() {
    map.setZoom(map.getZoom() - 1);
}

/*******************************************************************************
 * Decreases the zoom level of the map
 *******************************************************************************/
export function showIncidents(incidents) {
    for (let i = 0; i < incidents.length; i++) {
        const incident = incidents[i];
        const marker = new google.maps.Marker({
            position: new google.maps.LatLng(incident.lat, incident.lng),
            title: incident.reference,
            icon: {
                url: 'https://infralobo.city-platform.com/api/incidents/map/marker?icon=signpost&color=%234db300&restrict=false',
                scaledSize: new google.maps.Size(32, 32)
            },
            map,
        });
        marker.addListener("click", () => {
            infoWindow.close();
            infoWindow.setContent(marker.getTitle());
            infoWindow.open(marker.getMap(), marker);
        });
        markers.push(marker);
    }
}

/*******************************************************************************
 * Shows a message to the user if the browser does not support the geo-location.
 *******************************************************************************/
function handleLocationError(browserHasGeolocation, infoWindow, pos) {
    infoWindow.setPosition(pos);
    infoWindow.setContent(
        browserHasGeolocation
            ? "Erro: O serviço de geo-localização falhou."
            : "Erro: O seu browser não suporta geo-localização."
    );
    infoWindow.open(map);
}