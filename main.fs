include random.fs
include ansi-term.fs
BG_BLACK FG_WHITE ATTR_OFF 0 init-color-triple!

struct
	cell% field object-x
	cell% field object-y
	char% field object-graphics
	char% field object-symbol
end-struct object%

: objects  ( n -- n )
	object% %size * ;

: draw-object!  ( addr -- )
	dup object-x @ over object-y @ move-cursor! \ move the cursor to the right place
	dup object-graphics c@ set-graphics-by-index! \ get the right graphics mode
	object-symbol c@ emit \ display the object's symbol
;

: move-object!  ( x y addr -- )
	2dup object-y ! nip
	object-x !
;

: make-object!  ( x y graphics symbol addr -- )
	2dup object-symbol c! nip
	2dup object-graphics c! nip
	2dup object-y ! nip
	object-x !
;

: object-coordinates  ( addr -- x y )
	dup object-x @ swap object-y @ ;

\ width and height of our "map"
40 constant width
20 constant height

\ our main player
16 constant object-count
create game-objects object-count objects allot
: index->object  ( n -- n )
	objects game-objects + ;
0 value controlled-object

: for-objects!  ( xt -- )
	object-count 0 do
		i index->object
		over execute
	loop drop
;

: init-object!  { addr -- }
	width randint height randint 0
	27 randint [char] a +
	addr
	make-object!
;

: init-objects!  ( -- )
	['] init-object! for-objects!
;

init-objects!

\ 20 3 0 char @ controlled-object index->object make-object!
: run!  { game-over -- }
	begin
		erase-display!
		['] draw-object! for-objects!
		controlled-object index->object object-coordinates 2dup move-cursor!
		key case
			[char] k of 1- endof
			[char] j of 1+ endof
			[char] h of swap 1- swap endof
			[char] l of swap 1+ swap endof
			[char] q of -1 to game-over endof
		endcase
		controlled-object index->object move-object!
		graphics-normal!
	game-over until
;

0 run! page bye
