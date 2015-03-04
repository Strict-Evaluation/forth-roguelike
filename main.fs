include objects.fs
\ include ansi-term.fs
BG_BLACK FG_WHITE ATTR_OFF 0 init-color-triple!

0 value controlled-i
32 make-object-array game-objects[]
' game-objects[] value objs[]

: draw!  ( -- )
	erase-display!
	objs[] draw-objects!
	controlled-i game-objects[]
	object-coordinates move-cursor! ;

: run!  { game-over -- }
	objs[] . cr
	objs[] init-objects!
	begin
 		draw!
		controlled-i game-objects[] object-coordinates
		key case
			[char] k of 3 - endof
			[char] j of 1+ endof
			[char] h of swap 1- swap endof
			[char] l of swap 1+ swap endof
			[char] q of -1 to game-over endof
		endcase
 		controlled-i game-objects[] move-object!
		objs[] update-objects!
		graphics-normal!
	game-over until ;

0 run! page bye
