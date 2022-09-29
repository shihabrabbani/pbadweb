var totalSteps = $(".steps li").length;

$(".submit").on("click", function(){
  return false; 
});

$(".steps li:nth-of-type(1)").addClass("active");
$(".myContainer .form-container:nth-of-type(1)").addClass("active");

$(".form-container").on("click", ".next", function() { 
  $(".steps li").eq($(this).parents(".form-container").index() + 1).addClass("active"); 
  $(this).parents(".form-container").removeClass("active").next().addClass("active flipInX");   
});

$(".form-container").on("click", ".back", function() {  
  $(".steps li").eq($(this).parents(".form-container").index() - totalSteps).removeClass("active"); 
  $(this).parents(".form-container").removeClass("active flipInX").prev().addClass("active flipInY"); 
});

var totalSteps = $(".steps li").length;

$(".submit").on("click", function(){
  return false; 
});

$(".steps li:nth-of-type(1)").addClass("active");
$(".myContainer .form-container:nth-of-type(1)").addClass("active");

$(".form-container").on("click", ".next", function() { 
  $(".steps li").eq($(this).parents(".form-container").index() + 1).addClass("active"); 
  $(this).parents(".form-container").removeClass("active").next().addClass("active flipInX");   
});

$(".form-container").on("click", ".back", function() {  
  $(".steps li").eq($(this).parents(".form-container").index() - totalSteps).removeClass("active"); 
  $(this).parents(".form-container").removeClass("active flipInX").prev().addClass("active flipInY"); 
});

/******* Slider ********/

/******* date-picker ********/
$(document).ready(function () {

    $('.input-daterange').datepicker({
        format: 'dd-M-yyyy',
        autoclose: true
    });

});
