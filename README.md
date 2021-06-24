# A forked Jint Intepreter for .NET: an experiment to create a version of javascript that uses decimal types for numerics instead of IEEE doubles.  

### *"It's not just forked, it's fscked."*

## Why do this?
Because sometimes you need to do financial calculations, and having an embeddable language for doing so is useful.   IEEE floating point (floats/doubles) suffers from annoying rounding issues when doing monetary calculations; just open up node or the base jint and type "0.1 + 0.2" and see what you get as a result (Something like 0.30000000000000004 , because both numbers are infinitely repeating in binary representation).   Various workarounds for these rounding issues lead to difficult to maintain code and/or a source for constant and subtle bug reports.

So I wanted to see if I could create an embeddable language that would use decimals **_by default_** instead, so that e.g. 0.1+0.2==0.3.   This was an experiment in doing it with a javascript interpreter, using jint

## [Rant #1]
How come all math expression libs seem to assume you want doubles (either exclusively or by default), and don't allow for easy override to force the use of decimals?!?  Seems like an oversight....

## Why *NOT* to do this?
It's a bit broken version of JS.  Its nowhere near spec for JS.   Many unit tests fail.   *Some* transcendental functions (sin, cos,  etc) still sorta work, but return slightly different values (due to me taking the shortcut of converting from decimals to doubles and back).   Worse, since C# decimals don't have a notion of NaN, PositiveInfinity, NegativeInfinity, or positive vs negative zero (not sure?), various transcendental functions are broken for lots of values.   Scientific notation probably doesn't work right either, i didn't really fully explore that.   Really, most of the transcendental functions should be ripped out, and for financial calculations they aren't necessary anyway.  I probably screwed up date functions in the process.

~~Date functions are horribly broken due to math overflows, which I didn't expect.  This has broken almost all the unit tests~~ *Update: Fixed overflows, but fix is a little dubious. Unbroke over 6000 unit tests with a few tweaks here and there*

However, standard arithmetic functions (+, -, \*, /, %) for financial calculations should operate as expected for *decimal* types (no rounding nastyness).

Of course decimal calculations are a bit slower than floats, since IEEE floating point calculations are built into your FPU, decimals require explict algorithms implement.  Not something you'd want to write a video game with.

## How?
I took some shortcuts to port his quickly.  I created a "Money" struct to wrap a nullable decimal.   null is used internally to represent NaN.  I mainly did that to minimize the amount of code I needed to change, rather than for correctness.   The Money class also offers replacements for some things in System.Math that would operate on this new type.  You'll note some functions in there relating to Infinity that are again done for expediency, and are essentially incorrect.

## *Outcome of this experiment*
After about 4 hours of hacking at it, I created a frankenstein monster.  Left to it's own devices it may kill its creator.   I decided this was too heavyweight of a change to maintain and spec out, so for my purposes I mothballed this attempt and created a completely custom simplified language interpreter more suited to my purposes.

## Todo  (if I were to continue to persue this, which I probably won't)
- Get rid of all the Transcendental funcitions in Math that are broken
- figure out why the date calculations are overflowing, I think I've traced that to YearFromTime(), which works in an odd way
- Fix any remaining bugs (Particularly, conversions to and from string are kludged right now and probably dubious for some edge cases)
- eliminate all the NaN special handling and migrate from the Money wrapper directly to decimal
- Fix Unit tests (oh the humanity!), or removes ones that no longer apply. About 218 still fail, many related to missing concept of infinity, others may be more easily fixable
- Call it something other than Javascript
- Kill it with fire, mercifully


## Usage
Don't even think about using this in a production environment as is.  No telling how much fail is in there.  This is just an experiment

## [Rant #2]
Don't use floats or doubles for monetary calculations.  Ever.  Find an add-on library for your language that uses an internal decimal (rather than binary) representation if you have to.  Java: use BigDecimal, C#/VB.Net use decimal (primitive type, yay!).  Javascript: use Big.js or some equivalent, Python use decimal class,  etc.  Every decent language should have at least one, mature languages should ship with one.  
Sidenote: This one of the things the Groovy language got right:  any number that has a dot in it (like 0.1) is by default a BigDecimal, floats and doubles have to be explictly declared (0.1f or 0.1d).

An informal discussion of why floating point is bad for money calculations here: http://stackoverflow.com/questions/3730019/why-not-use-double-or-float-to-represent-currency/3730040#3730040
More thorough explanation here: https://docs.oracle.com/cd/E19957-01/806-3568/ncg_goldberg.html

## Clarification
The core issue with IEEE floating point for calculations involving cents isn't so much a rounding, its that numbers that are not infinitely repeating decimals ARE infinitely repeating binary numbers in IEEE representation.  Rounding just exposes this problem.  See the links above. e.g. 0.1 decimal == .000110011001100110011001100110011001100110011001100110011001... in binary.  See also http://www.exploringbinary.com/why-0-point-1-does-not-exist-in-floating-point/

## Contact
Don't send me bug reports.  You really shouldn't be using this.  This is a proof of concept, nothing more.


For the standard jint docs docs, see https://github.com/sebastienros/jint
