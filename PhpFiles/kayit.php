<?php

function Kayit(){
	$sorgu = mysql_query( "SELECT * FROM user_list" );
	$bugun= date('Y-m-d h:i:s');
	$ekle ="INSERT INTO user_list(kullanici,urunanahtari,durum,isim,soyisim,sonkullanma) VALUES 
		('".$_GET['kull']."',
		'".$_GET['key']."',
		'0',
		'".$_GET['isim']."',
		'".$_GET['soyisim']."',
		'".date('Y-m-d h:i:s',strtotime($bugun. ' +7 days'))."');" ;

        //echo mysql_error();
	while( $row = mysql_fetch_array( $sorgu ) ) {
		if( $_GET['kull'] == $row['kullanici']){
			if( $_GET['key'] == $row['urunanahtari']){
				echo "0";
				return;
			}
		}
	}
	if(mysql_query($ekle))
		echo "1";
	else
		echo "2";
	//echo mysql_error();
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
Kayit();
BaglantiKapat();
?>

		