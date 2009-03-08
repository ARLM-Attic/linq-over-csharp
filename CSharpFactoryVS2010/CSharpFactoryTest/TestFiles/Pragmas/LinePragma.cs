/*
 * Check line pragma
*/
// --- Next is OK.
#line 1000

#warning Warning 1

#line default

#warning Warning 2

#line
#line incorrect
#line "incorrect.cs"
#line xxx "incorrect.cs"

#line 2000 "correct.cs"

#warning Warning 3
