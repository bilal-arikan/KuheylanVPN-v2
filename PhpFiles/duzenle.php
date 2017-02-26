<?php

function Duzenle(){
	$bugun= time('Y-m-d h:i:s');
	$tabloAdi = "user_list";
	if($_GET['islem'] == 'listele')
	{
		$sorgu = mysql_query( "SELECT * FROM ".$tabloAdi );
		echo "<table><tbody>";
		while( $row = mysql_fetch_array( $sorgu ) ) {
			echo '<tr>'.
			'<td>'.$row['id'].'</td>'.
			'<td>'.$row['kullanici'].'</td>'.
			'<td>'.$row['urunanahtari'].'</td>'.
			'<td>'.$row['durum'].'</td>'.
			'<td>'.$row['isim'].'</td>'.
			'<td>'.$row['soyisim'].'</td>'.
			'<td>'.$row['sonkullanma'].'</td>'.
			'</tr>';
		}
		echo "</tbody></table>";
	}
	else if($_GET['islem'] == 'degistir')
	{
		$sorgu = "UPDATE ".$tabloAdi." SET ".
			"kullanici='"	.$_GET['kullanici']."',".
			"urunanahtari='".$_GET['urunanahtari']."',".
			"durum="		.$_GET['durum'].",".
			"isim='"		.$_GET['isim']."',".
			"soyisim='"		.$_GET['soyisim']."',".
			"sonkullanma='"	.$_GET['sonkullanma']."' ".
			"WHERE id=".$_GET['id'].";";
		
		if(mysql_query($sorgu))
			echo "1";
		else
			echo "2";
	}
	elseif($_GET['islem'] == 'ekle')
	{
		$sorgu ="INSERT INTO ".$tabloAdi."(kullanici,urunanahtari,durum,isim,soyisim,sonkullanma) VALUES 
			('".$_GET['kullanici']."',
			'".$_GET['urunanahtari']."',
			'".$_GET['durum']."',
			'".$_GET['isim']."',
			'".$_GET['soyisim']."',
			'".$_GET['sonkullanma']."');" ;
			
		if(mysql_query($sorgu))
			echo "1";
		else
			echo "2";
	}
	else if($_GET['islem'] == 'sil')
	{
		$sorgu =  "DELETE FROM ".$tabloAdi.
		" WHERE id=".$_GET['id'].";";
		
		if(mysql_query($sorgu))
			echo "1";
		else
			echo "2";
	}
}
function Baglan(){
	$user = "kuheylanusername"
	$pass = "kuheylanpassword"
	$baglan = mysql_connect("localhost",$user,$pass);
	mysql_select_db("users_database",$baglan);
	if(!$baglan)
		echo "2";
}
function BaglantiKapat(){
	mysql_close();
}


Baglan();
Duzenle();
BaglantiKapat();
?>