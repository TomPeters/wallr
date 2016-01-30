var gulp = require('gulp');
var watch = require('gulp-watch');
var livereload = require('gulp-livereload');

gulp.task('default', ['watch-content']);

gulp.task('watch-content', function () {
    livereload.listen();
    return gulp.src('Wallr.UI/content/**/*')
        .pipe(watch('Wallr.UI/content/**/*'))
        .pipe(gulp.dest('Wallr.Windows10/bin/Debug/Content'))
        .pipe(livereload());
});

