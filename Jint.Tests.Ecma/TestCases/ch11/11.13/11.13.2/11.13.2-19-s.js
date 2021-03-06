/// Copyright (c) 2012 Ecma International.  All rights reserved. 
/**
 * @path ch11/11.13/11.13.2/11.13.2-19-s.js
 * @description Strict Mode - ReferenceError isn't thrown if the LeftHandSideExpression of a Compound Assignment operator(+=) evaluates to a resolvable reference
 * @onlyStrict
 */


function testcase() {
        "use strict";
        var _11_13_2_19 = -1
        _11_13_2_19 += 10;
        return _11_13_2_19 === 9;
    }
runTestCase(testcase);
