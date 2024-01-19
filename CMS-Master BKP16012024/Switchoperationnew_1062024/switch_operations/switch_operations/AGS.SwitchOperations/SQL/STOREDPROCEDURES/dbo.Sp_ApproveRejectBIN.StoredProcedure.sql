U S E   [ S w i t c h O p e r a t i o n s ]  
 G O  
 / * * * * * *   O b j e c t :     S t o r e d P r o c e d u r e   [ d b o ] . [ S p _ A p p r o v e R e j e c t B I N ]         S c r i p t   D a t e :   0 8 - 0 6 - 2 0 1 8   1 6 : 5 8 : 1 7   * * * * * * /  
 S E T   A N S I _ N U L L S   O N  
 G O  
 S E T   Q U O T E D _ I D E N T I F I E R   O N  
 G O  
  
 C R E A T E   P R O C E D U R E   [ d b o ] . [ S p _ A p p r o v e R e j e c t B I N ]  
       @ I D   I N T   = 0 ,  
       @ C h e c k e r I D   B I G I N T ,        
       @ F o r m S t a t u s I D   i n t ,  
       @ R e m a r k   v a r c h a r ( 5 0 ) = ' '  
 A S  
 B E G I N  
 	 B e g i n   T r a n s a c t i o n      
 	 B e g i n   T r y          
 	 	 	 D e c l a r e   @ S t r P r i O u t p u t   v a r c h a r ( 1 ) = ' 1 ' 	 	 	 	 	  
 	 	 	 D e c l a r e   @ S t r P r i O u t p u t D e s c   v a r c h a r ( 2 0 0 ) = ' B i n   i s   n o t   A p p r o v e d / R e j e c t e d '  
  
 	 	 	 I F   E x i s t s ( S E L E C T   1   F R O M   T b l B I N   W I T H ( N O L O C K )   W H E R E   I D = @ I D )  
 	 	 	 B E G I N  
 	 	 	 - - F o r m s t a t u s   1   A c c e p t    
 	 	 	 	 I f ( @ F o r m S t a t u s I D = 1 )  
 	 	 	 	   B E G I N 	 	                
  
 	 	 	 	       U P D A T E   T b l B I N   S E T   F o r m S t a t u s I D = @ F o r m S t a t u s I D , C h e c k e r I D = @ C h e c k e r I D , C h e c k e d D a t e = G E T D A T E ( ) , R e m a r k = ' '   W H E R E   I D = @ I D  
 	 	 	 	         S E T   @ S t r P r i O u t p u t = ' 0 '  
 	 	 	 	         S E T   @ S t r P r i O u t p u t D e s c = ' B i n   i s   a p p r o v e d '  
 	 	 	 	   E N D  
 	 	 	 - - F o r m s t a t u s   2   r e j e c t    
 	 	 	     E L S E   I f ( @ F o r m S t a t u s I D = 2 )  
 	 	 	 	   B E G I N  
 	 	 	                 U P D A T E   T b l B I N   S E T   F o r m S t a t u s I D = @ F o r m S t a t u s I D , C h e c k e r I D = @ C h e c k e r I D , C h e c k e d D a t e = G E T D A T E ( ) , R e m a r k = @ R e m a r k   W H E R E   I D = @ I D  
 	 	 	 	         S E T   @ S t r P r i O u t p u t = ' 0 '  
 	 	 	 	         S E T   @ S t r P r i O u t p u t D e s c = ' B i n   i s   r e j e c t e d '  
 	 	 	 	   E N D  
 	 	 	   E N D  
 	 	 	   E L S E  
 	 	 	   B E G I N  
 	 	 	       S E T   @ S t r P r i O u t p u t = ' 1 '  
 	 	 	       I F ( @ F o r m S t a t u s I D = 1 )  
 	 	 	         B E G I N  
 	 	 	         S E T   @ S t r P r i O u t p u t D e s c = ' B i n   i s   n o t   a p p r o v e d '  
 	 	 	       E N D  
 	 	 	       E L S E  
 	 	 	         B E G I N  
 	 	 	 	   S E T   @ S t r P r i O u t p u t D e s c = ' B i n   i s   n o t   r e j e c t e d '  
 	 	 	 	 E N D  
 	 	 	   E N D 	 	 	  
 	 	 	 	 	 	 S e l e c t   @ S t r P r i O u t p u t   A s   C o d e , @ S t r P r i O u t p u t D e s c   A s   [ O u t p u t D e s c r i p t i o n ]  
  
 	 	 C O M M I T   T R A N S A C T I O N ;          
         E n d   T r y      
 	   B E G I N   C A T C H    
 	   R o l l B A C K   T R A N S A C T I O N ;    
 	   	 	 S E L E C T   1     A s   C o d e , ' E r r o r   o c c u r s . '   A s   [ O u t p u t D e s c r i p t i o n ]  
 	      
 	 	 	 I N S E R T   I N T O   T b l E r r o r D e t a i l ( P r o c e d u r e _ N a m e , E r r o r _ D e s c , E r r o r _ D a t e )                                    
 	 	     S E L E C T   E R R O R _ P R O C E D U R E ( ) , E R R O R _ M E S S A G E ( ) + ' L i n e   N u m b e r : '   + c a s t ( E R R O R _ L I N E ( )   a s   v a r c h a r ( 5 0 ) ) , G E T D A T E ( )  
 	 	          
 	 E N D   C A T C H ;      
  
  
 E N D  
  
 G O  
 