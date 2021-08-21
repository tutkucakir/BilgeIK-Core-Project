$(document).ready(function(){
    // currency
    
    var slider2 = document.getElementById('nouislider-2');

    noUiSlider.create(slider2, {
        start: [ 20000 ],
        connect: [true, false],
        step: 1000,
        range: {
            'min': [ 20000 ],
            'max': [ 80000 ]
        },
        format: wNumb({
            decimals: 3,
            thousand: '.',
            postfix: ' (US $)',
        })
    });

    // init slider input
    var sliderInput2 = document.getElementById('nouislider-input-2');

    slider2.noUiSlider.on('update', function( values, handle ) {
        sliderInput2.value = values[handle];
    });

    sliderInput2.addEventListener('change', function(){
        slider2.noUiSlider.set(this.value);
    });



    // tooltip-slider 
    var slider = document.getElementById('nouislider-3');

    noUiSlider.create(slider, {
        start: [20, 80],
        connect: true,
        direction: 'rtl',
        tooltips: [true, wNumb({ decimals: 1 })],
        range: {
            'min': [0],
            '10%': [10, 10],
            '50%': [80, 50],
            '80%': 150,
            'max': 200
        }
    });
   

    // init slider input
    var sliderInput0 = document.getElementById('nouislider-input-3');
    var sliderInput1 = document.getElementById('nouislider-input-3.1');
    var sliderInputs = [sliderInput1, sliderInput0];        

    slider.noUiSlider.on('update', function( values, handle ) {
        sliderInputs[handle].value = values[handle];
    });


    
    // nouislider-4
    var slider = document.getElementById('nouislider-input-select');

        // Append the option elements
        for ( var i = -20; i <= 40; i++ ){

            var option = document.createElement("option");
                option.text = i;
                option.value = i;

            slider.appendChild(option);
        }

        // init slider
        var html5Slider = document.getElementById('nouislider-4');

        noUiSlider.create(html5Slider, {
            start: [ 10, 30 ],
            connect: true,
            range: {
                'min': -20,
                'max': 40
            }
        });

        // init slider input
        var inputNumber = document.getElementById('nouislider-input-number');

        html5Slider.noUiSlider.on('update', function( values, handle ) {

            var value = values[handle];

            if ( handle ) {
                inputNumber.value = value;
            } else {
                slider.value = Math.round(value);
            }
        });

        slider.addEventListener('change', function(){
            html5Slider.noUiSlider.set([this.value, null]);
        });

        inputNumber.addEventListener('change', function(){
            html5Slider.noUiSlider.set([null, this.value]);
        });

    
        // noUiSlider-5 
        var slider5 = document.getElementById('nouislider-5');

        noUiSlider.create(slider5, {
            start: 20,
            range: {
                min: 0,
                max: 100
            },
            pips: {
                mode: 'values',
                values: [20, 80],
                density: 4
            }
        });

        var sliderInput5 = document.getElementById('nouislider-input-5');

        slider5.noUiSlider.on('update', function( values, handle ) {
            sliderInput5.value = values[handle];
        });

        sliderInput5.addEventListener('change', function(){
            slider5.noUiSlider.set(this.value);
        });

        slider5.noUiSlider.on('change', function ( values, handle ) {
            if ( values[handle] < 20 ) {
                slider5.noUiSlider.set(20);
            } else if ( values[handle] > 80 ) {
                slider5.noUiSlider.set(80);
            }
        });


        // noUiSlider-6 
        var verticalSlider = document.getElementById('nouislider-6');

        noUiSlider.create(verticalSlider, {
            start: 40,
            orientation: 'vertical',
            range: {
                'min': 0,
                'max': 100
            }
        }); 

        // init slider input
        var sliderInput = document.getElementById('nouislider-input-6');

        verticalSlider.noUiSlider.on('update', function( values, handle ) {
            sliderInput.value = values[handle];
        });

        sliderInput.addEventListener('change', function(){
            verticalSlider.noUiSlider.set(this.value);
        });      

    
});
