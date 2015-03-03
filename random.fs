\ RNG courtesy of http://www.forth.com/starting-forth/sf10/sf10.html
variable rnd rnd rnd !
: random  
	rnd @ 31421 *  6927 +  dup rnd ! ;
: randint  ( u1 -- u2 )
	random um*  nip ;
