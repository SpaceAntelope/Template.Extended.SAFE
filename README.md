# Template.Extended.SAFE
The SAFE Template (with Fulma) extended with updated Fable/React references and integrating navigation, responsive navbar, footer with status messages and page loader functionality.

This is not meant as a replacement for the Template.SAFE, rather to provide a useful point of reference for certain ubiquitous functionality that is not available out of the box and is seemingly not otherwise available in the same place. It might also be useful to some people as a stopgap until the official template is updated to use the new fable compiler.

## Updated Fable references
Updated client references to fable-compiler 2.2 as per https://fable.io/blog/Announcing-2-2.html and fixed breaking changes, mainly by manually updating the paket references and webpack according to https://github.com/fable-compiler/webpack-config-template 

## Navigation
Use the url bar as input to your application, allowing you to directly navigate to a specific subsections of your Fable SPA, while updating browser history thus enabling use of the browser back and forward buttons for navigation. Also implemented a graceful fallback message for bad urls.

TO DO: Guide to adding a new page

## Properly responsive navbar
Out of the box, the contents of the Bulma navigation bar simply disappeared on narrower viewports. I added the hamburger button and updated the MVU to allow for the expected responsive functionality.

## Footer
A slim footer at the bottom of the root page that also works as a host for status notifications. 

## Page loader functionality
The Fulma page loader needed some not immediately obvious steps (well to me anyway...) to get it to work, such as updating the .sass, available here for your convenience. 

The activity indicator is implemented as a globally available message, i.e. that can be called from any sub section of the SPA when it wants to block the interface and signal busy-ness by dispatching a _ToggleBusy of bool_ message.

## Sub page friendly structure
A file tree geared towards an SPA sectioned along pages that appear as the content of a root page that itself implements any navigation, activity indicator, status messaging and other globally available functionality.

The MVU pattern of each page is separated in Type/State/View files with an optional Data.fs, a pattern I saw in https://github.com/MangelMaxime/fulma-demo and which I am finding increasingly convenient, in both Fable and Fabulous. I also have this repo to thank for showing proper use of the bulma page loader in fulma. 

Note that the *fulma-demo* project also uses an updated version of the fable references, so if this repo doesn't end up working out for you as far as that aspect goes, I encourage you take a look there as well.

## Caveat emptor
I have definitely not tested this template for production readiness, and very definitely not in a cloud or a container. Which is to say that while the fable-elmish code patterns should be good to go anywhere, I have so far not investigated what breaks and what works in the various deployment scenarios that the official template provides for, as far as referencial sturdiness and webpack packaging goes.

Also, despite updating the template for access to the newer react features, I have absolutely not done any work towards actually using them for optimization. 



