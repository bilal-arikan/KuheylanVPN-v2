<?php

$site1 = file_get_contents("http://www.freevpnaccess.com/");
$tablo1 = explode('<div class="CSSTable">',$site1);
$tablo1 = explode('</div>',$tablo1[1]);
//echo $tablo1[0];
$vpn1 = explode('<td>',$tablo1[0]);

//$site2 = file_get_contents("http://freecloudvpn.com/");
$tablo2 = explode('US VPN Hostname',$site2);
$tablo2 = explode('<center>',$tablo2[1]);
$vpn2 = explode(': ',$tablo2[0]);

$site4 = file_get_contents("http://www.vpnbook.com/");
$vpn4 = explode('<strong>Password: ',$site4);
$vpn4 = explode('</strong></li>',$vpn4[1]);

echo "<table><tbody>";

//##################################  www.vpnbook.com  #######################################
echo "
	<tr>
		<td>euro195.vpnbook.com</td>
		<td>PPTP</td>
		<td>vpnbook</td>
		<td>".$vpn4[0]."</td>
	</tr><tr>
		<td>euro213.vpnbook.com</td>
		<td>PPTP</td>
		<td>vpnbook</td>
		<td>".$vpn4[0]."</td>
	</tr><tr>
		<td>uk180.vpnbook.com</td>
		<td>L2TP</td>
		<td>vpnbook</td>
		<td>".$vpn4[0]."</td>
	</tr><tr>
		<td>us1.vpnbook.com</td>
		<td>L2TP</td>
		<td>vpnbook</td>
		<td>".$vpn4[0]."</td>
	</tr>
";
//##################################  freevpnaccess.com  #######################################
echo "		<tr>
			<td>".$vpn1[7]."
			<td>PPTP</td>
			<td>".$vpn1[9]."
			<td>".$vpn1[10]."

			<td>".$vpn1[12]."
			<td>PPTP</td>
			<td>".$vpn1[14]."
			<td>".$vpn1[15]."
			
			<td>".$vpn1[17]."
			<td>PPTP</td>
			<td>".$vpn1[19]."
			<td>".$vpn1[20]."

			<td>".$vpn1[22]."
			<td>L2TP</td>
			<td>".$vpn1[24]."
			<td>".$vpn1[25]."

			<td>".$vpn1[27]."
			<td>PPTP</td>
			<td>".$vpn1[29]."
			<td>".$vpn1[30]."

			<td>".$vpn1[32]."
			<td>L2TP</td>
			<td>".$vpn1[34]."
			<td>".strip_tags($vpn1[35])."</td>
		</tr>
";
//##################################  freecloudvpn.com  #######################################
echo "
	<tr>
		<td>us.freecloudvpn.com</td>
		<td>PPTP</td>
		<td>freecloudvpn.com</td>
		<td>".str_replace('</p>
<p>
	UK VPN Hostname', '', $vpn2[3])."</td>
	</tr><tr>
		<td>uk.freecloudvpn.com</td>
		<td>PPTP</td>
		<td>freecloudvpn.com</td>
		<td>".str_replace('</p>
', '', $vpn2[6])."</td>
	</tr>";

/*
$site3 = file_get_contents("http://freevpn.me/accounts");
$vpn3 = explode('<b>Password:</b> ',$site3);
$vpn3 = explode('</li>',$vpn3[1]);

//##################################  freevpn.me  #######################################
echo "
	<tr>
		<td>93.115.83.250</td>
		<td>PPTP</td>
		<td>pptp</td>
		<td>".$vpn3[0]."</td>
	</tr>
";*/
echo "</tbody></table>";
?>