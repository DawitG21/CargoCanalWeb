﻿/// <binding AfterBuild='build' />
const gulp = require("gulp");
const image = require('gulp-image');
const replace = require("gulp-replace");
const htmlmin = require('gulp-htmlmin');
const log = require('fancy-log');


const versionRegEx = /build=([0-9]*.[0-9]*.[0-9]*)/g;
const headRef = /^<!--head-refs-start-->(.*)<!--head-refs-end-->$/;
const bodyRef = /^<!--body-refs-start-->(.*)<!--body-refs-end-->$/;


let filesToMove = {
    'css': ['content/style-bundle.min.css', 'content/style-bundle-defer.min.css', 'content/datatables.min.css', 'content/common-sprite.sprite.*'],
    'datatable_img': ['bower_components/datatables/media/images/*.*'],
    'fonts': ['fonts/**/*.*'],
    'open_iconic': ['content/open-iconic/**/*.*'],
    'image': ['content/images/**/*.*'],
    'content': ['content/themes/**/*.*'],
    'lib': ['scripts/bundles/*.js',
        'scripts/app/cards.min.js',
        'scripts/geez/jgeez.min.js'
    ],
    'root_clean': ['favicon.png'],
    'root': ['views/**/*.*', 'templates_quarantined/**/*.*', 'index.html', 'manifest.json', 'robots.txt'],
    'index': 'index.html',
    'webconfig': ['Web.config']
};



gulp.task('moveimages', [], function () {
    // the base option sets the relative root for the set of files,
    // preserving the folder structure
    gulp.src(filesToMove.image, { base: 'content/images' })
        .pipe(image())
        .pipe(gulp.dest('src/assets/images'));
});

gulp.task('movedatatable_img', ['moveimages'], function () {
    gulp.src(filesToMove.datatable_img, { base: 'bower_components/' })
        .pipe(image())
        .pipe(gulp.dest('src/assets/bower_components'));
});

gulp.task('moveassets', ['movedatatable_img'], function () {
    gulp.src(filesToMove.content, { base: 'content/' })
        .pipe(gulp.dest('src/assets/images'));
});

gulp.task('movecss', ['moveassets'], function () {
    gulp.src(filesToMove.css, { base: 'content/' })
        .pipe(gulp.dest('src/assets/css'));
});

gulp.task('movefont', ['movecss'], function () {
    gulp.src(filesToMove.fonts)
        .pipe(gulp.dest('src/assets/fonts'));
});

gulp.task('moveopeniconic', ['movefont'], function () {
    gulp.src(filesToMove.open_iconic)
        .pipe(gulp.dest('src/assets/open-iconic'));
});

gulp.task('movelib', ['moveopeniconic'], function () {
    gulp.src(filesToMove.lib)
        .pipe(gulp.dest('src/lib'));
});


gulp.task('dist_clean', ['movelib'], function () {
    gulp.src(['src/**/*.*', '!src/**/*.js', '!src/**/*.css', '!src/**/*.html'], { base: 'src/' })
        .pipe(gulp.dest('dist'));
});


gulp.task('dist', ['dist_clean'], function () {
    gulp.src(['src/**/*.js', 'src/**/*.css', '!src/assets/images/**/*.*'], { base: 'src/' })
        .pipe(replace('src/', ''))
        .pipe(replace('http://localhost:52931', 'https://appliedline.com/cargocanalapi'))
        .pipe(gulp.dest('dist'));
});

gulp.task('root_clean', ['dist'], function () {
    gulp.src(filesToMove.root_clean, { base: './' })
        .pipe(gulp.dest('dist'));
});

gulp.task('web_config', ['root_clean'], function () {
    gulp.src(filesToMove.webconfig, { base: './' })
        .pipe(replace('<!--<rule', '<rule'))
        .pipe(replace('</rule>-->', '</rule>'))
        .pipe(gulp.dest('dist'));
});

function appVersionPart(text) {
    return text.substring(text.indexOf('.') + 1);
}

gulp.task('increase_build_version', ['web_config'], function () {
    let appVersion = {
        'major': 0,
        'minor': 0,
        'build': 0
    };

    gulp.src(filesToMove.index, { base: './' })
        .pipe(replace(versionRegEx, (match, p1) => {
            appVersion.major = parseInt(p1.substring(0, p1.indexOf('.')));

            let text = appVersionPart(p1);
            appVersion.minor = parseInt(text.substring(0, text.indexOf('.')));

            text = appVersionPart(text);
            appVersion.build = parseInt(text);

            return `build=${appVersion.major}.${appVersion.minor}.${appVersion.build + 1}`;
        }))
        //.on('end', () => { console.log('Here loging leke'); })
        .pipe(gulp.dest('./'))
        .pipe(replace('src/', ''))
        .pipe(gulp.dest('dist'));
});

gulp.task('build', ['increase_build_version'], function () {
    gulp.src(filesToMove.root, { base: './' })

        .pipe(replace('src/', ''))
        //.pipe(htmlmin({ collapseWhitespace: true }))
        .pipe(gulp.dest('dist'));
});

//gulp.task('build', ['root_clean'], function () {
//    gulp.src(filesToMove.root, { base: './' })
//        .pipe(replace('src/', ''))
//        .pipe(replace('<!--<rule', '<rule'))
//        .pipe(replace('</rule>-->', '</rule>'))
//        .pipe(gulp.dest('dist'));
//});

// gulp to automatically re-run the scripts task whenever js files change
gulp.task('watch', function () {
    return gulp.watch('scripts/**/*.js', ['build']);
});