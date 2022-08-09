let hasFeatures = false;
let drawingManager;
let selectedTool;
let colors = ['#1E90FF', '#FF1493', '#32CD32', '#FF8C00', '#4B0082'];
let selectedShape;
let shapeInClipboard;
let dotNetObjRef;

window.plugin1MapExtensions = {

    zoomIn: function () {
        let map = window.gso.map;
        map.setZoom(map.getZoom() + 1);
    },

    zoomOut: function () {
        let map = window.gso.map;
        map.setZoom(map.getZoom() - 1);
    },

    loadGeoJson: function () {
        let map = window.gso.map;
        if (!hasFeatures) {
            map.data.loadGeoJson('/plugins/resources/sample.json');
            hasFeatures = true;
        } else {
            map.data.forEach((feature) => {
                map.data.remove(feature);
            });
            hasFeatures = false;
        }
    },

    initializeDrawingTools: function (objRef) {
        dotNetObjRef = objRef;

        let map = window.gso.map;
        drawingManager = new google.maps.drawing.DrawingManager({
            drawingMode: google.maps.drawing.OverlayType.POLYGON,
            drawingControl: false,
            markerOptions: {
                draggable: true
            },
            polylineOptions: {
                editable: true,
                draggable: true
            },
            rectangleOptions: {
                strokeWeight: 0,
                fillOpacity: 0.45,
                editable: true,
                draggable: true
            },
            circleOptions: {
                strokeWeight: 0,
                fillOpacity: 0.45,
                editable: true,
                draggable: true
            },
            polygonOptions: {
                strokeWeight: 0,
                fillOpacity: 0.45,
                editable: true,
                draggable: true
            },
            map: map
        });

        // Opens the map context menu
        google.maps.event.addListener(map, 'contextmenu', (ev) => {
            dotNetObjRef.invokeMethodAsync('ShowContextMenu', ev.domEvent.clientX, ev.domEvent.clientY, 'map');
        });

        google.maps.event.addListener(drawingManager, 'overlaycomplete', (e) => {
            const newShape = e.overlay;
            // Add a new type property to the shape object.
            // This is used later on to identity the type of the shape.
            newShape.type = e.type;

            this.addShape(newShape);
        });

        // Clear the current selection when the drawing mode is changed, or when the
        // map is clicked.
        google.maps.event.addListener(drawingManager, 'drawingmode_changed', this.clearSelection);
        // TODO: Fix this
        //map.onMapClick.subscribe(() => this.clearSelection());
        this.setColor(colors[0]);
        drawingManager.setDrawingMode(null);
        this.setDrawingTool('pan');
    },

    setDrawingTool: function (tool) {
        selectedTool = tool;
        this.clearSelection();
        switch (tool) {
            case 'circle':
            case 'marker':
            case 'polygon':
            case 'polyline':
            case 'rectangle':
                drawingManager.setDrawingMode(tool);
                break;
            case 'pan':
                drawingManager.setDrawingMode(null);
                break;
            case 'delete': break;
            default:
                break;
        }
    },

    clearSelection: function () {
        if (selectedShape) {
            if (selectedShape.type !== 'marker') {
                selectedShape.setEditable(false);
            }

            selectedShape = null;
        }
    },

    setSelection: function (shape) {
        if (shape.type !== 'marker') {
            this.clearSelection();
            shape.setEditable(true);
            this.selectColor(shape.get('fillColor') || shape.get('strokeColor'));
        }

        selectedShape = shape;
    },

    copySelectedShape: function () {
        console.log('inside copy #1');
        if (selectedShape) {
            console.log('inside copy #2');
            shapeInClipboard = selectedShape;
        }
    },

    pasteSelectedShape: function () {
        console.log('inside paste #1');
        if (shapeInClipboard) {
            console.log('inside paste #2');
            const clone = this.cloneShape(shapeInClipboard)
            this.addShape(clone);
            this.setSelection(clone);
            shapeInClipboard = clone;
        }
    },

    deleteSelectedShape: function () {
        if (selectedShape) {
            selectedShape.setMap(null);
        }
    },

    selectColor: function (color) {
        selectedColor = color;

        // Retrieves the current options from the drawing manager and replaces the
        // stroke or fill color as appropriate.
        const polylineOptions = drawingManager.get('polylineOptions');
        polylineOptions.strokeColor = color;
        drawingManager.set('polylineOptions', polylineOptions);

        const rectangleOptions = drawingManager.get('rectangleOptions');
        rectangleOptions.fillColor = color;
        drawingManager.set('rectangleOptions', rectangleOptions);

        const circleOptions = drawingManager.get('circleOptions');
        circleOptions.fillColor = color;
        drawingManager.set('circleOptions', circleOptions);

        const polygonOptions = drawingManager.get('polygonOptions');
        polygonOptions.fillColor = color;
        drawingManager.set('polygonOptions', polygonOptions);
    },

    setSelectedShapeColor: function (color) {
        if (selectedShape) {
            if (selectedShape.type === google.maps.drawing.OverlayType.POLYLINE) {
                selectedShape.set('strokeColor', color);
            } else {
                selectedShape.set('fillColor', color);
            }
        }
    },

    setColor: function (color) {
        this.selectColor(color);
        this.setSelectedShapeColor(color);
    },

    addShape(shape) {
        if (shape.type !== google.maps.drawing.OverlayType.MARKER) {
            // Switch back to non-drawing mode after drawing a shape.
            selectedTool = 'pan';
            drawingManager.setDrawingMode(null);

            // Add an event listener that selects the newly-drawn shape when the user
            // mouses down on it.
            google.maps.event.addListener(shape, 'contextmenu', (ev) => {
                dotNetObjRef.invokeMethodAsync('ShowContextMenu', ev.domEvent.clientX, ev.domEvent.clientY, 'shape');
            });

            // Add an event listener that selects the newly-drawn shape when the user
            // mouses down on it.
            google.maps.event.addListener(shape, 'click', (ev) => {

                if (ev.domEvent.altKey) {
                // Create a copy of the shape on click
                const clone = this.cloneShape(shape)
                this.addShape(clone);
                this.setSelection(clone);
                    
                }
                if (ev.vertex !== undefined) {

                    if (shape.type === google.maps.drawing.OverlayType.POLYGON) {
                        const path = shape.getPaths().getAt(ev.path);
                        path.removeAt(ev.vertex);
                        if (path.length < 3) {
                            shape.setMap(null);
                        }
                    }
                    if (shape.type === google.maps.drawing.OverlayType.POLYLINE) {
                        const path = shape.getPath();
                        path.removeAt(ev.vertex);
                        if (path.length < 2) {
                            shape.setMap(null);
                        }
                    }
                }
                this.setSelection(shape);
            });

            this.setSelection(shape);
        }
        else {
            google.maps.event.addListener(shape, 'click', () => {
                this.setSelection(shape);
            });
            this.setSelection(shape);
        }
    },

    cloneShape(shape) {
        let clone = null;

        switch (shape.type) {
            case google.maps.drawing.OverlayType.POLYGON:
                const offsetPaths = [];
                for (let i = 0; i < shape.getPath().length; i++) {
                    const path = shape.getPath().getAt(i);
                    const offsetPath = this.offsetCoordinate(path, 50, 50);
                    offsetPaths.push(offsetPath);
                }
                clone = new google.maps.Polygon({
                    paths: offsetPaths,
                    strokeWeight: shape.strokeWeight,
                    fillColor: shape.fillColor,
                    fillOpacity: shape.fillOpacity,
                    map: shape.getMap(),
                    editable: true,
                    draggable: true
                });
                break;
            case google.maps.drawing.OverlayType.POLYLINE:
                console.log('-> POLYLINE');
                break;
            case google.maps.drawing.OverlayType.CIRCLE:
                const offsetCenter = this.offsetCoordinate(shape.getCenter(), 50, 50);
                debugger;
                clone = new google.maps.Circle({
                    strokeWeight: shape.strokeWeight,
                    fillColor: shape.fillColor,
                    fillOpacity: shape.fillOpacity,
                    center: offsetCenter,
                    radius: shape.getRadius(),
                    map: shape.getMap(),
                    editable: true,
                    draggable: true
                });
                break;
            case google.maps.drawing.OverlayType.RECTANGLE:
                debugger;
                const bounds = shape.getBounds();
                const offsetBounds = {
                    north: this.offsetCoordinate(bounds.getNorthEast(), 50, 50).lat(),
                    south: this.offsetCoordinate(bounds.getSouthWest(), 50, 50).lat(),
                    east: this.offsetCoordinate(bounds.getNorthEast(), 50, 50).lng(),
                    west: this.offsetCoordinate(bounds.getSouthWest(), 50, 50).lng()
                };
                console.log(bounds.toJSON());
                console.log(offsetBounds);
                clone = new google.maps.Rectangle({
                    strokeWeight: shape.strokeWeight,
                    fillColor: shape.fillColor,
                    fillOpacity: shape.fillOpacity,
                    center: offsetCenter,
                    radius: shape.getRadius(),
                    bounds: offsetBounds,
                    map: shape.getMap(),
                    editable: true,
                    draggable: true
                });
                console.log('-> RECTANGLE');
                break;
            default:
                console.log('-> OTHER');
                break;
        }

        if (clone) {
            // Add a new type property to the shape object.
            // This is used later on to identity the type of the shape.
            clone.type = shape.type;
        }

        return clone;
        // console.log('adding')
        // var p = new google.maps.Polygon();
        // p.setPaths(newShape.getPaths());
        // p.setMap(map);

        // console.log('added')
        // console.log(newShape.getPaths())
    },

    offsetCoordinate(latlng, offsetx, offsety) {
        // latlng is the apparent centre-point
        // offsetx is the distance you want that point to move to the right, in pixels
        // offsety is the distance you want that point to move upwards, in pixels
        // offset can be negative
        // offsetx and offsety are both optional
        const map = window.gso.map;
        const scale = Math.pow(2, map.getZoom());
        const projectedCoordinate = map.getProjection().fromLatLngToPoint(latlng);
        const pixelOffset = new google.maps.Point((offsetx / scale) || 0, (offsety / scale) || 0);
        const newCoordinate = new google.maps.Point(
            projectedCoordinate.x - pixelOffset.x,
            projectedCoordinate.y + pixelOffset.y
        );

        return map.getProjection().fromPointToLatLng(newCoordinate);
    },
};