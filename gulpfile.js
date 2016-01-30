var gulp = require('gulp');
var watch = require('gulp-watch');

gulp.task('watch-content', function() {
    return gulp.src('Wallr.UI/content/**/*')
        .pipe(watch('Wallr.UI/content/**/*'))
        .pipe(gulp.dest('Wallr.Windows10/bin/Debug/Content'));
});

