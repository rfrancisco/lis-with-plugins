let hasFeatures = false;
let drawingManager;
let selectedTool;
let colors = ['#1E90FF', '#FF1493', '#32CD32', '#FF8C00', '#4B0082'];
let selectedShape;

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

    initializeDrawingTools: function () {
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

        google.maps.event.addListener(drawingManager, 'overlaycomplete', (e) => {
            const newShape = e.overlay;
            newShape.type = e.type;

            if (e.type !== google.maps.drawing.OverlayType.MARKER) {
                // Switch back to non-drawing mode after drawing a shape.
                selectedTool = 'pan';
                drawingManager.setDrawingMode(null);

                // Add an event listener that selects the newly-drawn shape when the user
                // mouses down on it.
                google.maps.event.addListener(newShape, 'click', (ev) => {
                    if (ev.vertex !== undefined) {
                        if (newShape.type === google.maps.drawing.OverlayType.POLYGON) {
                            const path = newShape.getPaths().getAt(ev.path);
                            path.removeAt(ev.vertex);
                            if (path.length < 3) {
                                newShape.setMap(null);
                            }
                        }
                        if (newShape.type === google.maps.drawing.OverlayType.POLYLINE) {
                            const path = newShape.getPath();
                            path.removeAt(ev.vertex);
                            if (path.length < 2) {
                                newShape.setMap(null);
                            }
                        }
                    }
                    this.setSelection(newShape);
                });
                this.setSelection(newShape);
            }
            else {
                google.maps.event.addListener(newShape, 'click', () => {
                    this.setSelection(newShape);
                });
                this.setSelection(newShape);
            }
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
    }
};