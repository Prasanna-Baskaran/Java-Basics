 
 A L T E R   p r o c e d u r e   [ d b o ] . [ U S P _ M a k e C a r d H o t L i s t ]     - - e x e c   [ U S P _ M a k e C a r d H o t L i s t ]   ' ' , @ M o d e = ' s e l e c t '  
 (  
 	 @ I d   i n t = 0 ,  
 	 @ M o d e   v a r c h a r ( 5 0 ) ,  
 	 @ O l d C a r d R P A N I D   v a r c h a r ( 5 0 0 ) = n u l l  
 )  
 a s  
 b e g i n  
 - - d e c l a r e   @ M o d e   v a r c h a r ( 5 0 0 )  
 - - d e c l a r e   @ C h e c k e d I D L i s t   v a r c h a r ( 5 0 0 ) = ' 1 , 2 , 3 , 4 , 5 '  
 - - s e l e c t   *   f r o m   T b l C a r d G e n R e q u e s t  
 i f ( @ M o d e = ' s e l e c t ' )  
 B e g i n  
 	 s e l e c t   a . I D , C u s t o m e r I D , N e w B i n P r e f i x , d b o . u f n _ D e c r y p t P A N ( D e c P A N ) [ c a r d n o ] , I R . S w i t c h I s s N r [ I s s u e r N o ] , c p . e n c p a n   O l d C a r d R P A N I D    
 	 f r o m   T b l C a r d G e n R e q u e s t _ H i s t o r y   a   w i t h   ( N o l o c k )  
 	 I n n e r   j o i n   C a r d R P A N   C P   w i t h   ( N o l o c k )   o n   a . O l d C a r d R P A N I D = c p . I D  
 	 I n n e r   j o i n   I s s u e r _ N r     I R   w i t h   ( N o l o c k )   o n   c p . I s s u e r N o = I R . I s s u e r N r  
 	 w h e r e   I s A u t h o r i z e d = 1   a n d   H o l d R S P C o d e   i n   ( ' 4 1 ' , ' 4 3 ' )   a n d   I S N U L L ( R s p c o d e , ' ' )   < > ' 0 0 '  
 	 u n i o n    
 	 s e l e c t   a . I D , C u s t o m e r I D , N e w B i n P r e f i x , o l d c a r d r p a n i d [ c a r d n o ] , I R . S w i t c h I s s N r [ I s s u e r N o ] , c p . e n c p a n     O l d C a r d R P A N I D      
 	 f r o m   T b l C a r d G e n R e q u e s t   a   w i t h   ( N o l o c k )  
 	 I n n e r   j o i n   C a r d R P A N   C P   w i t h   ( N o l o c k )   o n   a . O l d C a r d R P A N I D = c p . I D  
 	 I n n e r   j o i n   I s s u e r _ N r     I R   w i t h   ( N o l o c k )   o n   c p . I s s u e r N o = I R . I s s u e r N r  
 	 w h e r e   I s A u t h o r i z e d = 1   a n d   H o l d R S P C o d e   i n   ( ' 4 1 ' , ' 4 3 ' )   a n d   I S N U L L ( R s p c o d e , ' ' )   < > ' 0 0 '  
 E n d  
 E l s e  
 i f ( @ M o d e = ' u p d a t e ' )  
 	 B e g i n  
 	 	 U p d a t e   T b l C a r d G e n R e q u e s t   s e t   R S P C o d e   =   ' 0 0 '     w h e r e   i d   = @ I d   a n d   o l d p a n   =   @ O l d C a r d R P A N I D   a n d   p r o c e s s i d = 6  
 	 	 U p d a t e   T b l C a r d G e n R e q u e s t H i s t o r y   s e t   R S P C o d e   =   ' 0 0 '   w h e r e   i d   = @ I d   a n d   o l d p a n   =   @ O l d C a r d R P A N I D   a n d   p r o c e s s i d = 6  
 	 	 s e l e c t   ' s u c c e s s '  
 	 E n d  
 E n d  
  
 - - s e l e c t   *   f r o m   T b l C A C a r d G e n P r o c e s s T y p e s  
 - - s e l e c t   *   f r o m   T b l C a r d G e n R e q u e s t _ H i s t o r y  
 - - s e l e c t   *   f r o m   T b l C a r d G e n R e q u e s t  
  
  
 