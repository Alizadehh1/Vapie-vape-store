$(function() {

    $('.toast').toast('show');

    $('.nav-item.dropdown').mouseenter(function() {
        $(this).addClass('show');
        $(this).children('.dropdown-menu').addClass('show');
        $(this).children('.dropdown-toggle').attr('aria-expanded', 'true');
    }).mouseleave(function() {
        $(this).removeClass('show');
        $(this).children('.dropdown-menu').removeClass('show');
        $(this).children('.dropdown-toggle').attr('aria-expanded', 'false');
    });

    $('.img-small').on('mouseenter click', function() {
        var src = $(this).data('src');
        $('.img-large').css("background-image", "url('"+src+"')");
    });

    var imgLarge = $('.img-large');

    imgLarge.mousemove(function(event) {
        var relX = event.pageX - $(this).offset().left;
        var relY = event.pageY - $(this).offset().top;
        var width = $(this).width();
        var height = $(this).height();
        var x = (relX / width) * 100;
        var y = (relY / height) * 100;
        $(this).css("background-position", x+"% "+y+"%");
    });

    imgLarge.mouseout(function() {
        $(this).css("background-position", "center");
    });

    $( window ).resize(function() {
        setImgLarge();
        setImgSmall();
    });

    setImgLarge();
    setImgSmall();

});

function setImgLarge() {
    var imgLarge = $('.img-large');
    var width = imgLarge.width();
    imgLarge.height(width * 2/3);
}

function setImgSmall() {
    var imgSmall = $('.img-small');
    var width = imgSmall.width();
    imgSmall.height(width);
}

$(function () {
    $(".acc_ctrl").on("click", function (e) {
        e.preventDefault();
        if ($(this).hasClass("active")) {
            $(this).removeClass("active");
            $(this).next().stop().slideUp(300);
        } else {
            $(this).addClass("active");
            $(this).next().stop().slideDown(300);
        }
    });
});

document.querySelectorAll('.logoutButton').forEach(button => {
    button.state = 'default'

    // function to transition a button from one state to the next
    let updateButtonState = (button, state) => {
        if (logoutButtonStates[state]) {
            button.state = state
            for (let key in logoutButtonStates[state]) {
                button.style.setProperty(key, logoutButtonStates[state][key])
            }
        }
    }

    // mouse hover listeners on button
    button.addEventListener('mouseenter', () => {
        if (button.state === 'default') {
            updateButtonState(button, 'hover')
        }
    })
    button.addEventListener('mouseleave', () => {
        if (button.state === 'hover') {
            updateButtonState(button, 'default')
        }
    })

    // click listener on button
    button.addEventListener('click', () => {
        if (button.state === 'default' || button.state === 'hover') {
            button.classList.add('clicked')
            updateButtonState(button, 'walking1')
            setTimeout(() => {
                button.classList.add('door-slammed')
                updateButtonState(button, 'walking2')
                setTimeout(() => {
                    button.classList.add('falling')
                    updateButtonState(button, 'falling1')
                    setTimeout(() => {
                        updateButtonState(button, 'falling2')
                        setTimeout(() => {
                            updateButtonState(button, 'falling3')
                            setTimeout(() => {
                                button.classList.remove('clicked')
                                button.classList.remove('door-slammed')
                                button.classList.remove('falling')
                                updateButtonState(button, 'default')
                            }, 1000)
                        }, logoutButtonStates['falling2']['--walking-duration'])
                    }, logoutButtonStates['falling1']['--walking-duration'])
                }, logoutButtonStates['walking2']['--figure-duration'])
            }, logoutButtonStates['walking1']['--figure-duration'])
        }
    })
})

const logoutButtonStates = {
    'default': {
        '--figure-duration': '100',
        '--transform-figure': 'none',
        '--walking-duration': '100',
        '--transform-arm1': 'none',
        '--transform-wrist1': 'none',
        '--transform-arm2': 'none',
        '--transform-wrist2': 'none',
        '--transform-leg1': 'none',
        '--transform-calf1': 'none',
        '--transform-leg2': 'none',
        '--transform-calf2': 'none'
    },
    'hover': {
        '--figure-duration': '100',
        '--transform-figure': 'translateX(1.5px)',
        '--walking-duration': '100',
        '--transform-arm1': 'rotate(-5deg)',
        '--transform-wrist1': 'rotate(-15deg)',
        '--transform-arm2': 'rotate(5deg)',
        '--transform-wrist2': 'rotate(6deg)',
        '--transform-leg1': 'rotate(-10deg)',
        '--transform-calf1': 'rotate(5deg)',
        '--transform-leg2': 'rotate(20deg)',
        '--transform-calf2': 'rotate(-20deg)'
    },
    'walking1': {
        '--figure-duration': '300',
        '--transform-figure': 'translateX(11px)',
        '--walking-duration': '300',
        '--transform-arm1': 'translateX(-4px) translateY(-2px) rotate(120deg)',
        '--transform-wrist1': 'rotate(-5deg)',
        '--transform-arm2': 'translateX(4px) rotate(-110deg)',
        '--transform-wrist2': 'rotate(-5deg)',
        '--transform-leg1': 'translateX(-3px) rotate(80deg)',
        '--transform-calf1': 'rotate(-30deg)',
        '--transform-leg2': 'translateX(4px) rotate(-60deg)',
        '--transform-calf2': 'rotate(20deg)'
    },
    'walking2': {
        '--figure-duration': '400',
        '--transform-figure': 'translateX(17px)',
        '--walking-duration': '300',
        '--transform-arm1': 'rotate(60deg)',
        '--transform-wrist1': 'rotate(-15deg)',
        '--transform-arm2': 'rotate(-45deg)',
        '--transform-wrist2': 'rotate(6deg)',
        '--transform-leg1': 'rotate(-5deg)',
        '--transform-calf1': 'rotate(10deg)',
        '--transform-leg2': 'rotate(10deg)',
        '--transform-calf2': 'rotate(-20deg)'
    },
    'falling1': {
        '--figure-duration': '1600',
        '--walking-duration': '400',
        '--transform-arm1': 'rotate(-60deg)',
        '--transform-wrist1': 'none',
        '--transform-arm2': 'rotate(30deg)',
        '--transform-wrist2': 'rotate(120deg)',
        '--transform-leg1': 'rotate(-30deg)',
        '--transform-calf1': 'rotate(-20deg)',
        '--transform-leg2': 'rotate(20deg)'
    },
    'falling2': {
        '--walking-duration': '300',
        '--transform-arm1': 'rotate(-100deg)',
        '--transform-arm2': 'rotate(-60deg)',
        '--transform-wrist2': 'rotate(60deg)',
        '--transform-leg1': 'rotate(80deg)',
        '--transform-calf1': 'rotate(20deg)',
        '--transform-leg2': 'rotate(-60deg)'
    },
    'falling3': {
        '--walking-duration': '500',
        '--transform-arm1': 'rotate(-30deg)',
        '--transform-wrist1': 'rotate(40deg)',
        '--transform-arm2': 'rotate(50deg)',
        '--transform-wrist2': 'none',
        '--transform-leg1': 'rotate(-30deg)',
        '--transform-leg2': 'rotate(20deg)',
        '--transform-calf2': 'none'
    }
}




