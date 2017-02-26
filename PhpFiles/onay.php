<?php

function Onay(){
	$simdikiZaman = time('Y-m-d h:i:s');
	$sorgu = mysql_query( "SELECT * FROM user_list" );
        //echo mysql_error();
	while( $row = mysql_fetch_array( $sorgu ) ) {
		if( $_GET['kull'] == $row['kullanici']){
			if( $_GET['key'] == $row['urunanahtari']){
				if( $row['durum'] == 1 || $row['durum'] == 0){
					if( strtotime($row['sonkullanma']) > $simdikiZaman){
						echo "1";
						$kalanSaniye 	= strtotime($row['sonkullanma']) - $simdikiZaman;
						echo $kalanSaniye;
						return;
					}else{
						echo "4";
						return;
					}	
				}else{
					echo "3";
					return;
				}
			}
		}
	}
        echo "0";
}
function Baglan(){
	$user = "kuheylanusername"
	$pass = "kuheylanpassword"
	$baglan = mysql_connect("localhost",$user,$pass);
	mysql_select_db("genel",$baglan);
	if(!$baglan)
		echo "2";
}
function BaglantiKapat(){
	mysql_close();
}


Baglan();
Onay();
BaglantiKapat();
?>