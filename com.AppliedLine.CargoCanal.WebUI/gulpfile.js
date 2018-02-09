/// <binding AfterBuild='build' />
const gulp = require("gulp");
const image = require('gulp-image');
const replace = require("gulp-replace");
const htmlmin = require('gulp-htmlmin');

var filesToMove = {
    'css': ['content/style-bundle.min.css', 'content/style-bundle-defer.min.css', 'content/datatables.min.css', 'content/common-sprite.sprite.*'],
    'datatable_img': ['bower_components/datatables/media/images/*.*'],
    'fonts': ['fonts/*.*'],
    'image': ['content/images/**/*.*'],
    'content': ['content/themes/**/*.*'],
    'lib': ['scripts/bundles/*.min.js',
        'scripts/bundles/angulars_and_chartjs.js',
        'scripts/app/cards.min.js',
        'scripts/geez/jgeez.min.js'
    ],
    'root_clean': ['favicon.png'],
    'root': ['views/**/*.*', 'templates_quarantined/**/*.*', 'index.html', 'manifest.webmanifest'],
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

gulp.task('movelib', ['movefont'], function () {
    gulp.src(filesToMove.lib)
        .pipe(gulp.dest('src/lib'));
});


gulp.task('dist_clean', ['movelib'], function () {
    gulp.src(['src/**/*.*', '!src/**/*.js', '!src/**/*.css', '!src/**/*.html'], { base: 'src/' })
        .pipe(gulp.dest('dist'));
})

//gulp.task('dist', ['dist_clean'], function () {
//    gulp.src(['src/**/*.js', 'src/**/*.css', 'src/**/*.html', '!src/assets/images/**/*.*'], { base: 'src/' })
//        .pipe(replace('src/', ''))
//        .pipe(replace('http://localhost:49931', 'https://appliedline.com/cargocanalapi'))
//        .pipe(gulp.dest('dist'));
//})

gulp.task('dist', ['dist_clean'], function () {
    gulp.src(['src/**/*.js', 'src/**/*.css', '!src/assets/images/**/*.*'], { base: 'src/' })
        .pipe(replace('src/', ''))
        .pipe(replace('http://localhost:49931', 'https://appliedline.com/cargocanalapi'))
        .pipe(gulp.dest('dist'));
})

gulp.task('root_clean', ['dist'], function () {
    gulp.src(filesToMove.root_clean, { base: './' })
        .pipe(gulp.dest('dist'));
})

gulp.task('web_config', ['root_clean'], function () {
    gulp.src(filesToMove.webconfig, { base: './' })
        .pipe(replace('<!--<rule', '<rule'))
        .pipe(replace('</rule>-->', '</rule>'))
        .pipe(gulp.dest('dist'));
});

gulp.task('build', ['web_config'], function () {
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