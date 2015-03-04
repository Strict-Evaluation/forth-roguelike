include random.fs
include ansi-term.fs
include constants.fs

struct
	cell% field object-x
	cell% field object-y
	cell% field object-update
	char% field object-graphics
	char% field object-symbol
end-struct object%

: objects  ( n -- n )
	object% %size * ;
: draw-object!  ( addr -- )
	\ move the cursor to the right place
	dup object-x @ over object-y @ move-cursor! 
	\ get the right graphics mode
	dup object-graphics c@ set-graphics-by-index! 
	\ display the object's symbol
	object-symbol c@ emit ;
: move-object!  ( x y addr -- )
	2dup object-y ! nip
	object-x ! ;
: make-object!  ( x y graphics symbol update-fn addr -- )
	2dup object-update ! nip 
	2dup object-symbol c! nip
	2dup object-graphics c! nip
	2dup object-y ! nip
	object-x ! ;
: update-object!  ( addr -- )
	dup object-update @ execute ;
: object-coordinates  ( addr -- x y )
	dup object-x @ swap object-y @ ;

: index->object  ( addr n -- addr )
	objects + ;

: for-objects!  { addr -- ( xt count addr -- ) }
	0 do
		addr i index->object
		over execute
	loop drop ;

: default-update  ( addr -- )
	object-y dup @ 1+ dup height < if else drop height then
	swap ! ;

: init-object!  { addr -- }
	width randint height randint 0
	27 randint [char] a +
	['] default-update
	addr
	make-object! ;

: init-objects!  ( count addr -- )
	['] init-object! -rot for-objects! ;

: draw-objects!  ( count addr -- )
	['] draw-object! -rot for-objects! ;

: update-objects!  ( count addr -- )
	['] update-object! -rot for-objects! ;

: make-object-array ( length [name] -- )
	create dup here ! objects cell+ allot
	does>  ( index -- addr )
		swap objects cell+ + ;
