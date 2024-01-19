 
 A L T E R   f u n c t i o n   [ d b o ] . [ U d f _ G e t C a r d D e t a i l s T o C h a r g e ] ( @ f r o m   d a t e t i m e , @ t o   d a t e t i m e , @ i s s u e r N o   n u m e r i c ( 9 ) , @ t y p e   c h a r ( 1 ) )      
 r e t u r n s   @ r e t u r n C a r d F e e D a t a   t a b l e      
 ( C a r d N o   v a r c h a r ( 2 0 0 ) , A c c o u n t N o   v a r c h a r ( 2 0 0 ) ,      
   d a t e _ i s s u e d   d a t e t i m e ,   c u s t o m e r _ i d   v a r c h a r ( 1 0 0 ) ,   h o l d _ r s p _ c o d e   v a r c h a r ( 1 0 ) ,      
   E n c P A N     v a r c h a r ( 2 0 0 ) ,     C u s t o m e r _ N a m e     v a r c h a r ( 1 0 0 0 ) ,   l a s t _ u p d a t e d _ d a t e   d a t e t i m e , e x p i r y _ d a t e   v a r c h a r ( 8 ) )      
 a s      
 / *          
   D e s c r i p t i o n :   T h i s   f u n c t i o n   p r o v i d e   b a n k w i s e   c u s t o m e r   d e t a i l s      
   A u t h o r :   P r a t i k   M h a t r e            
   C r e a t e   D a t e :   3 0 - 0 8 - 2 0 1 7      
   M o d i f i e d   D a t e :        
   M o d i f i c a t i o n :            
 * /          
 b e g i n      
      
 / * G e t t i n g   C u s t o m e r   I d   w h i c h   a r e   m a t c h i n g   i n   i s s u e i n g   c o n d i t i o n * /      
 D e c l a r e   @ C u s t I d T a b l e   t a b l e   ( D a t a   v a r c h a r ( 1 0 0 ) )      
 i f   @ t y p e = ' I '     / * I s s u a n c e   F e e   a f t e r   3 0   d a y s * /  
 b e g i n      
 	   i n s e r t   i n t o   @ C u s t I d T a b l e        
 	   s e l e c t   d i s t i n c t   c u s t o m e r _ i d   f r o m   S y n c G e n e r i c C a r d D e t a i l s   w i t h ( n o l o c k )   w h e r e   d a t e _ i s s u e d   b e t w e e n   @ f r o m   a n d   @ t o   - - a n d   i s s u e r n o = @ i s s u e r N o      
 e n d      
 e l s e   i f ( @ t y p e = ' R ' )     / * R e i s s u e a n c e   F e e   o n   n e x t   d a y s * /  
 b e g i n      
 	   i n s e r t   i n t o   @ C u s t I d T a b l e        
 	   - - s e l e c t   d i s t i n c t   c u s t o m e r _ i d     f r o m     S y n c G e n e r i c C a r d D e t a i l s   w i t h ( n o l o c k )   w h e r e   d a t e _ i s s u e d   b e t w e e n   @ f r o m   a n d   @ t o   - - a n d   i s s u e r n o = @ i s s u e r N o      
 	     s e l e c t   d i s t i n c t   c . c u s t o m e r _ i d     f r o m   T b l C a r d G e n R e q u e s t _ H i s t o r y   r   w i t h ( n o l o c k )      
 	     i n n e r   j o i n   C a r d R P A N   c   w i t h ( n o l o c k )   o n   r . O l d P a n = c . E n c P A N      
 	     a n d   c . c u s t o m e r _ i d   i n   ( s e l e c t   d i s t i n c t   c u s t o m e r _ i d     f r o m     S y n c G e n e r i c C a r d D e t a i l s   w i t h ( n o l o c k )   w h e r e   d a t e _ i s s u e d   b e t w e e n   @ f r o m   a n d   @ t o )      
 e n d      
 e l s e      
 b e g i n     / * A n n u a l   F e e   -   o n   d a i l y   b a s i s   f e e   g e t   c h a r g e d   i t s   n o t   m o n t h l y   * /  
 	   i n s e r t   i n t o   @ C u s t I d T a b l e        
 	   s e l e c t   d i s t i n c t   c u s t o m e r _ i d     f r o m     S y n c G e n e r i c C a r d D e t a i l s   w i t h ( n o l o c k )   w h e r e        
 	   D A T E P A R T ( M M , d a t e _ i s s u e d   ) = D A T E P A R T ( M M , @ f r o m )   a n d        
 	   D A T E P A R T ( D D , d a t e _ i s s u e d   ) = D A T E P A R T ( D D , @ f r o m )   a n d      
 	   D a t e p a r t ( Y Y Y Y ,   d a t e _ i s s u e d ) < > d a t e p a r t ( Y Y Y Y , @ f r o m )  
 	   - - D A T E P A R T ( Y Y Y Y , d a t e _ i s s u e d )   n o t   b e t w e e n     D A T E P A R T ( Y Y Y Y , D A T E A D D ( Y Y Y Y ,   - 1 , @ f r o m ) )      
 	   - - a n d   D A T E P A R T ( Y Y Y Y , @ f r o m )        
   - - a n d   i s s u e r n o = @ i s s u e r N o      
 e n d      
        
 d e c l a r e   @ c a r d F e e D a t a     t a b l e ( C a r d N o   v a r c h a r ( 2 0 0 ) , A c c o u n t N o   v a r c h a r ( 2 0 0 ) , d a t e _ i s s u e d   d a t e t i m e ,     c u s t o m e r _ i d   v a r c h a r ( 1 0 0 ) ,   h o l d _ r s p _ c o d e   v a r c h a r ( 1 0 ) ,      
                     E n c P A N     v a r c h a r ( 2 0 0 ) ,     C u s t o m e r _ N a m e     v a r c h a r ( 1 0 0 0 ) ,   l a s t _ u p d a t e d _ d a t e   d a t e t i m e , e x p i r y _ d a t e   v a r c h a r ( 8 ) )        
   i n s e r t   i n t o   @ c a r d F e e D a t a      
   s e l e c t   C a r d N o ,     A c c o u n t N o ,   d a t e _ i s s u e d ,     c u s t o m e r _ i d   ,   h o l d _ r s p _ c o d e   , E n c P A N   ,     C u s t o m e r _ N a m e ,   l a s t _ u p d a t e d _ d a t e , e x p i r y _ d a t e      
   f r o m   (      
 	     S e l e c t     R O W _ N U M B E R ( )   o v e r ( p a r t i t i o n   b y   A . D e c P A N   o r d e r   b y   l a s t _ u p d a t e d _ d a t e   d e s c )   R W   ,   d b o . u f n _ D e c r y p t P A N ( A . D e c P A N )   C a r d N o ,      
 	     d b o . u f n _ D e c r y p t P A N ( E . D e c A c c )   A c c o u n t N o   ,     S . c u s t o m e r _ i d   , S . h o l d _ r s p _ c o d e   ,      
 	     E n c P A N   , S . C u s t o m e r _ N a m e   C u s t o m e r _ N a m e , s . l a s t _ u p d a t e d _ d a t e , d a t e _ i s s u e d , s . e x p i r y _ d a t e , d a t e _ d e l e t e d        
 	     F r o m        
 	     S y n c G e n e r i c C a r d D e t a i l s   S   w i t h ( n o l o c k )   I N N E R   J O I N   @ C u s t I d T a b l e   D       O N   S . c u s t o m e r _ i d = D . D a t a  
 	     i n n e r   j o i n   C a r d R P A N   A   W i t h   ( N o L o c k )   o n   A . I s s u e r N o = @ i s s u e r N o   A N D   A . E n c P A N = S . p a n _ e n c r y p t e d   - - a n d   s . i s s u e r N o = @ i s s u e r N o      
 	     i n n e r   J o i n   C a r d R A c c o u n t s   E   W i t h   ( N o L o c k )   O n   e . I s s u e r N o = @ i s s u e r N o   A N D   S . a c c o u n t _ i d _ e n c r y p t e d   C o l l a t e   S Q L _ L a t i n 1 _ G e n e r a l _ C P 1 _ C I _ A S = E . E n c A c c   C o l l a t e   S Q L _ L a t i n 1 _ G e n e r a l _ C P 1 _ C I _ A S        
 	     W h e r e      
 	       - - s . c u s t o m e r _ i d   i n   ( s e l e c t   D a t a   f r o m   @ C u s t I d T a b l e )     a n d     	        
 	     a c c o u n t _ t y p e _ q u a l i f i e r = 1   - - a n d   B . h o l d _ r s p _ c o d e   n o t   i n   ( 4 1 , 4 3      
   ) a   w h e r e   a . R W = 1        
   a n d   d a t e _ d e l e t e d   i s   n u l l      
   - - o r d e r   b y   a . c u s t o m e r _ i d , a . d a t e _ i s s u e d        
        
   i n s e r t   i n t o   @ r e t u r n C a r d F e e D a t a      
   s e l e c t   *   f r o m   @ c a r d F e e D a t a        
   r e t u r n      
 e n d  
  
 