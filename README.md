# README #

This project is a sample for a simple entity mapper using custom attributes. 

The idea is to have a little utility to map values from your DTO or POCO entities to viewmodels without having to type assignments for every field, and without tampering with the POCO entities... 

By using custom attributes you assign to your viewmodel it's POCO class and it's properties the "brother" property in the POCO entity.

It works both sides:

```
#!c#

MyPOCO entity = viewModel.MapTo<MyPOCO>();

ViewModel myModel = pocoEntity.MapTo<ViewModel>();

```


### What is this repository for? ###

* Learn how to create custom Attributes 
* Learn how to assign them to your classes
* Learn how to use reflection to get the attributes and use their values

### How do I get set up? ###

* Download Visual Studio 2013 Communitiy Edition (free to use)
* Clone the repository using Team Explorer
* Compile and go!

### Who do I talk to? ###

* To me! joan.vilarino at gmail.com 
* Don't forget to visit my blog at http://www.joanvilarino.info