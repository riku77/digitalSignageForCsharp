   M S B A M L   `   `   `  ����D  @pptTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:pptTester	pptTester  5   	   [ WSystem.Windows.Forms, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms 5   	   e aMicrosoft.Web.WebView2.Wpf, Version=1.0.1774.30, Culture=neutral, PublicKeyToken=2a8ab48044d2601ekLclr-namespace:Microsoft.Web.WebView2.Wpf;assembly=Microsoft.Web.WebView2.WpfMicrosoft.Web.WebView2.Wpf 5   	       pptTester.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:pptTester     5   	   Owf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     5   	   \wv2Lclr-namespace:Microsoft.Web.WebView2.Wpf;assembly=Microsoft.Web.WebView2.Wpf     5   	     � Title$  
MainWindow��5
   	   $	��640��6   $
��1280��6)    J� InputBindings 5      �� 5   
   
 �� Key$ I��6    �� 	Modifiers$ Controlf�6    �� Command 61   �� *OpenSettingsCommand+.��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��2 �6   �� 5      $	��260 �6    � ColumnDefinitions 5   
   �� 5      $��* �6   .+�5   
   �� $��1��6   $��Stretch=�6$   � 5    
   $��0��6    5!      �� 5"      $��* �6#   �� 5#      $��2 �6#   �� 5$      $��* �6#   .+�5'      �� $��1��6   $��Stretch=�6+   �� 5+      
�������._�5/      r� -   5         r�Name$ videoPlayer��5/       J� RenderTransformOrigin$ 0.5,0.5&�65   	 r� LoadedBehavior$	 Manual=�6z   50      52      �� 56      $��2��6   
����ح�._�57      r� -   5        $ videoPlayer2��57      $ 0.5,0.5&�66   $	 Manual=�6{   58      59      5:      �� 5?   
   $��2��6   
������._�5A      )  #Microsoft.Web.WebView2.Wpf.WebView2  -   5        
  Name$
 webView1��5A         Source$  ?�6-   5B      �� 5C   
   -   5         ��Name$ text1��5C      $�� ��6#   $��20(�6+   $��Center=�69   �� 5D   
   -   5        $ txtInfo��5D      
�������$��20(�68   $
��Bold"�6F   $��Center=�6X   $��Center=�6u    �� TextAlignment$ Center=�6�   6�   5E      5H      