// Wait for DOM to fully load before executing scripts
document.addEventListener('DOMContentLoaded', function () {
    // Initialize all interactive elements
    initNavigation();
    initSmoothScroll();
    initSkillBars();
    initProjectFilters();
    initThemeToggle();
    initContactForm();
    initScrollToTop();
    initTypewriterEffect();
});

// Mobile navigation functionality
function initNavigation() {
    const menuToggle = document.querySelector('.menu-toggle') || document.querySelector('.hamburger');
    const navMenu = document.querySelector('nav ul') || document.querySelector('.nav-menu');

    if (menuToggle && navMenu) {
        menuToggle.addEventListener('click', function () {
            navMenu.classList.toggle('active');
            menuToggle.classList.toggle('active');
        });

        // Close menu when clicking outside
        document.addEventListener('click', function (event) {
            if (!navMenu.contains(event.target) && !menuToggle.contains(event.target) && navMenu.classList.contains('active')) {
                navMenu.classList.remove('active');
                menuToggle.classList.remove('active');
            }
        });

        // Add active class to current page link
        const currentPage = window.location.pathname.split('/').pop();
        const navLinks = document.querySelectorAll('nav a');

        navLinks.forEach(link => {
            const linkPage = link.getAttribute('href').split('/').pop();
            if (linkPage === currentPage || (currentPage === '' && linkPage === 'index.html')) {
                link.classList.add('active');
            }

            // Close mobile menu when a link is clicked
            link.addEventListener('click', function () {
                if (window.innerWidth <= 768) {
                    navMenu.classList.remove('active');
                    menuToggle.classList.remove('active');
                }
            });
        });
    }
}

// Smooth scrolling for anchor links
function initSmoothScroll() {
    const anchorLinks = document.querySelectorAll('a[href^="#"]:not([href="#"])');

    anchorLinks.forEach(link => {
        link.addEventListener('click', function (e) {
            e.preventDefault();

            const targetId = this.getAttribute('href');
            const targetElement = document.querySelector(targetId);

            if (targetElement) {
                // Calculate header height for offset
                const headerHeight = document.querySelector('header')?.offsetHeight || 0;

                window.scrollTo({
                    top: targetElement.offsetTop - headerHeight - 20,
                    behavior: 'smooth'
                });
            }
        });
    });
}

// Animate skill bars on scroll
function initSkillBars() {
    const skillBars = document.querySelectorAll('.skill-bar .progress');

    if (skillBars.length > 0) {
        const animateSkills = function () {
            skillBars.forEach(bar => {
                const barPosition = bar.getBoundingClientRect().top;
                const screenPosition = window.innerHeight / 1.3;

                if (barPosition < screenPosition) {
                    const percentage = bar.getAttribute('data-progress') || bar.parentElement.getAttribute('data-progress');
                    if (percentage) {
                        bar.style.width = percentage + '%';
                    }
                }
            });
        };

        // Initial check
        animateSkills();

        // Check on scroll
        window.addEventListener('scroll', animateSkills);
    }
}

// Project filtering functionality
function initProjectFilters() {
    const filterButtons = document.querySelectorAll('.filter-btn');
    const projectItems = document.querySelectorAll('.project-item');

    if (filterButtons.length > 0 && projectItems.length > 0) {
        filterButtons.forEach(button => {
            button.addEventListener('click', function () {
                // Remove active class from all buttons
                filterButtons.forEach(btn => btn.classList.remove('active'));

                // Add active class to clicked button
                this.classList.add('active');

                const filter = this.getAttribute('data-filter');

                projectItems.forEach(item => {
                    if (filter === 'all') {
                        item.style.display = 'block';
                        setTimeout(() => {
                            item.classList.add('show');
                        }, 10);
                    } else if (item.classList.contains(filter)) {
                        item.style.display = 'block';
                        setTimeout(() => {
                            item.classList.add('show');
                        }, 10);
                    } else {
                        item.classList.remove('show');
                        setTimeout(() => {
                            item.style.display = 'none';
                        }, 300);
                    }
                });
            });
        });

        // Set "All" as active by default
        const allButton = document.querySelector('.filter-btn[data-filter="all"]');
        if (allButton) {
            allButton.classList.add('active');
        }
    }
}

// Light/Dark theme toggle
function initThemeToggle() {
    const themeToggle = document.querySelector('.theme-toggle');

    if (themeToggle) {
        // Check for saved theme
        const savedTheme = localStorage.getItem('theme') || 'light';
        document.body.classList.add(savedTheme);

        if (savedTheme === 'dark') {
            themeToggle.classList.add('active');
        }

        themeToggle.addEventListener('click', function () {
            this.classList.toggle('active');

            if (document.body.classList.contains('dark')) {
                document.body.classList.replace('dark', 'light');
                localStorage.setItem('theme', 'light');
            } else {
                document.body.classList.replace('light', 'dark');
                localStorage.setItem('theme', 'dark');
            }
        });
    }
}

// Contact form validation and submission
function initContactForm() {
    const contactForm = document.querySelector('#contact-form');

    if (contactForm) {
        contactForm.addEventListener('submit', function (e) {
            e.preventDefault();

            // Get form fields
            const nameInput = contactForm.querySelector('#name');
            const emailInput = contactForm.querySelector('#email');
            const messageInput = contactForm.querySelector('#message');
            const statusMessage = contactForm.querySelector('.form-status') || document.createElement('div');

            if (!contactForm.querySelector('.form-status')) {
                statusMessage.className = 'form-status';
                contactForm.appendChild(statusMessage);
            }

            // Simple validation
            let isValid = true;

            if (!nameInput.value.trim()) {
                showError(nameInput, 'Name is required');
                isValid = false;
            } else {
                clearError(nameInput);
            }

            if (!emailInput.value.trim()) {
                showError(emailInput, 'Email is required');
                isValid = false;
            } else if (!isValidEmail(emailInput.value)) {
                showError(emailInput, 'Please enter a valid email');
                isValid = false;
            } else {
                clearError(emailInput);
            }

            if (!messageInput.value.trim()) {
                showError(messageInput, 'Message is required');
                isValid = false;
            } else {
                clearError(messageInput);
            }

            if (isValid) {
                // In a real project, you would send data to a server here
                // For now, we'll simulate a successful submission
                statusMessage.textContent = 'Thank you! Your message has been sent.';
                statusMessage.className = 'form-status success';
                contactForm.reset();

                // Clear success message after 5 seconds
                setTimeout(() => {
                    statusMessage.textContent = '';
                    statusMessage.className = 'form-status';
                }, 5000);
            }
        });

        // Helper functions for form validation
        function showError(input, message) {
            const formGroup = input.parentElement;
            const errorElement = formGroup.querySelector('.error-message') || document.createElement('span');

            if (!formGroup.querySelector('.error-message')) {
                errorElement.className = 'error-message';
                formGroup.appendChild(errorElement);
            }

            errorElement.textContent = message;
            input.classList.add('error');
        }

        function clearError(input) {
            const formGroup = input.parentElement;
            const errorElement = formGroup.querySelector('.error-message');

            if (errorElement) {
                errorElement.textContent = '';
            }

            input.classList.remove('error');
        }

        function isValidEmail(email) {
            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            return emailRegex.test(email);
        }
    }
}

// Scroll to top button
function initScrollToTop() {
    const scrollBtn = document.querySelector('.scroll-top') || document.createElement('button');

    if (!document.querySelector('.scroll-top')) {
        scrollBtn.className = 'scroll-top';
        scrollBtn.innerHTML = '&uarr;';
        scrollBtn.setAttribute('aria-label', 'Scroll to top');
        scrollBtn.style.display = 'none';
        document.body.appendChild(scrollBtn);
    }

    window.addEventListener('scroll', function () {
        if (window.pageYOffset > 300) {
            scrollBtn.style.display = 'block';
        } else {
            scrollBtn.style.display = 'none';
        }
    });

    scrollBtn.addEventListener('click', function () {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    });
}

// Typewriter effect for hero section
function initTypewriterEffect() {
    const typewriterElement = document.querySelector('.typewriter');

    if (typewriterElement) {
        const phrases = typewriterElement.getAttribute('data-phrases')?.split(',') ||
            ['Web Developer', 'UI/UX Designer', 'Problem Solver'];
        let currentPhraseIndex = 0;
        let currentCharIndex = 0;
        let isDeleting = false;
        let typingSpeed = 100;

        function type() {
            const currentPhrase = phrases[currentPhraseIndex];

            if (isDeleting) {
                // Deleting text
                typewriterElement.textContent = currentPhrase.substring(0, currentCharIndex - 1);
                currentCharIndex--;
                typingSpeed = 50; // Delete faster than typing
            } else {
                // Typing text
                typewriterElement.textContent = currentPhrase.substring(0, currentCharIndex + 1);
                currentCharIndex++;
                typingSpeed = 100;
            }

            // If finished typing current phrase
            if (!isDeleting && currentCharIndex === currentPhrase.length) {
                isDeleting = true;
                typingSpeed = 1000; // Pause before starting to delete
            }

            // If finished deleting current phrase
            if (isDeleting && currentCharIndex === 0) {
                isDeleting = false;
                currentPhraseIndex = (currentPhraseIndex + 1) % phrases.length;
                typingSpeed = 500; // Pause before typing next phrase
            }

            setTimeout(type, typingSpeed);
        }

        // Start the typewriter effect
        setTimeout(type, 1000);
    }
}

// Project image preview/modal functionality
document.querySelectorAll('.project-item').forEach(item => {
    const projectImage = item.querySelector('img');

    if (projectImage) {
        projectImage.addEventListener('click', function () {
            const modal = document.createElement('div');
            modal.className = 'modal';

            const modalContent = document.createElement('div');
            modalContent.className = 'modal-content';

            const closeBtn = document.createElement('span');
            closeBtn.className = 'close-modal';
            closeBtn.innerHTML = '&times;';
            closeBtn.setAttribute('aria-label', 'Close preview');

            const modalImage = document.createElement('img');
            modalImage.src = this.src;
            modalImage.alt = this.alt;

            modalContent.appendChild(closeBtn);
            modalContent.appendChild(modalImage);
            modal.appendChild(modalContent);
            document.body.appendChild(modal);

            // Prevent scrolling when modal is open
            document.body.style.overflow = 'hidden';

            // Close modal when clicking close button or outside the modal
            modal.addEventListener('click', function (e) {
                if (e.target === modal || e.target === closeBtn) {
                    document.body.removeChild(modal);
                    document.body.style.overflow = '';
                }
            });

            // Close modal with Escape key
            document.addEventListener('keydown', function (e) {
                if (e.key === 'Escape' && document.querySelector('.modal')) {
                    document.body.removeChild(document.querySelector('.modal'));
                    document.body.style.overflow = '';
                }
            });
        });
    }
});

// Animate elements on scroll
const animateElements = document.querySelectorAll('.animate-on-scroll');

if (animateElements.length > 0) {
    function checkAnimation() {
        animateElements.forEach(element => {
            const elementPosition = element.getBoundingClientRect().top;
            const screenPosition = window.innerHeight / 1.2;

            if (elementPosition < screenPosition) {
                element.classList.add('animated');
            }
        });
    }

    // Initial check
    checkAnimation();

    // Check on scroll
    window.addEventListener('scroll', checkAnimation);
}