/// <binding AfterBuild='copy2dist, copy2usis' />
/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');

gulp.task('copy2dist', function () {
    gulp.src([
        'bin/Debug/dnx46/**'
    ]).pipe(gulp.dest('../../dist/'));
});

gulp.task('copy2usis', function () {
    gulp.src([
        '../../artifacts/bin/RoboUtil/Release/dnx46/**'
    ]).pipe(gulp.dest('../../../USIS/Usis/Main/packages/lib'));
});


 